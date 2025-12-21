import { createClient, type Client, type InStatement, type ResultSet } from '@libsql/client/web';

/**
 * Represents a database connection with its client and prepared statements.
 */
interface ConnectionInfo {
    client: Client;
    url: string;
    authToken?: string;
    syncUrl?: string;
    syncAuthToken?: string;
    isSyncEnabled: boolean;
    statements: Map<string, InStatement>;
}

/**
 * Result of a sync operation.
 */
interface SyncResult {
    framesSynced: number;
    durationMs: number;
    success: boolean;
    errorMessage?: string;
}

/**
 * Manages multiple database connections, one per database name.
 */
const connections = new Map<string, ConnectionInfo>();

/**
 * Counter for generating unique statement IDs.
 */
let statementCounter = 0;

/**
 * Generates a unique ID for connections and statements.
 */
function generateId(): string {
    return crypto.randomUUID();
}

// ============================================================================
// Database Lifecycle
// ============================================================================

/**
 * Connects to a Turso database.
 * @param databaseName - Unique identifier for this connection
 * @param url - Database URL (file path for local, https for remote)
 * @param authToken - Optional auth token for remote databases
 * @returns The connection ID
 */
export async function connect(
    databaseName: string,
    url: string,
    authToken?: string
): Promise<string> {
    // Close existing connection if any
    if (connections.has(databaseName)) {
        await disconnect(databaseName);
    }

    const client = createClient({
        url: url,
        authToken: authToken
    });

    connections.set(databaseName, {
        client,
        url,
        authToken,
        isSyncEnabled: false,
        statements: new Map()
    });

    return databaseName;
}

/**
 * Connects to a Turso database with sync support for embedded replicas.
 * @param databaseName - Unique identifier for this connection
 * @param url - Local database URL (file path)
 * @param authToken - Optional auth token for local database
 * @param syncUrl - Remote Turso Cloud URL for syncing
 * @param syncAuthToken - Auth token for Turso Cloud
 * @param encryptionKey - Optional encryption key for the local database
 * @returns The connection ID
 */
export async function connectWithSync(
    databaseName: string,
    url: string,
    authToken: string | undefined,
    syncUrl: string,
    syncAuthToken: string,
    encryptionKey?: string
): Promise<string> {
    // Close existing connection if any
    if (connections.has(databaseName)) {
        await disconnect(databaseName);
    }

    // Create client with sync configuration
    const clientConfig: any = {
        url: url,
        syncUrl: syncUrl,
        authToken: syncAuthToken
    };

    if (encryptionKey) {
        clientConfig.encryptionKey = encryptionKey;
    }

    const client = createClient(clientConfig);

    connections.set(databaseName, {
        client,
        url,
        authToken,
        syncUrl,
        syncAuthToken,
        isSyncEnabled: true,
        statements: new Map()
    });

    return databaseName;
}

/**
 * Disconnects from a database and cleans up resources.
 * @param databaseName - The connection ID to disconnect
 */
export async function disconnect(databaseName: string): Promise<void> {
    const conn = connections.get(databaseName);
    if (conn) {
        conn.statements.clear();
        conn.client.close();
        connections.delete(databaseName);
    }
}

/**
 * Checks if a database connection exists and is active.
 * @param databaseName - The connection ID to check
 * @returns True if connected
 */
export function isConnected(databaseName: string): boolean {
    return connections.has(databaseName);
}

// ============================================================================
// SQL Execution
// ============================================================================

/**
 * Result of an execute operation.
 */
interface ExecuteResult {
    rowsAffected: number;
    lastInsertRowId: number;
}

/**
 * Executes a SQL statement that modifies data (INSERT, UPDATE, DELETE, etc.).
 * @param databaseName - The connection ID
 * @param sql - SQL statement to execute
 * @param params - Parameters for the SQL statement
 * @returns Execution result with rowsAffected and lastInsertRowId
 */
export async function execute(
    databaseName: string,
    sql: string,
    params: any[] = []
): Promise<ExecuteResult> {
    const conn = getConnection(databaseName);
    const result = await conn.client.execute({ sql, args: params });

    return {
        rowsAffected: result.rowsAffected,
        lastInsertRowId: Number(result.lastInsertRowid ?? 0)
    };
}

/**
 * Executes a SQL query and returns all matching rows.
 * @param databaseName - The connection ID
 * @param sql - SQL query to execute
 * @param params - Parameters for the SQL query
 * @returns Array of row objects
 */
export async function query<T = any>(
    databaseName: string,
    sql: string,
    params: any[] = []
): Promise<T[]> {
    const conn = getConnection(databaseName);
    const result = await conn.client.execute({ sql, args: params });

    return rowsToObjects<T>(result);
}

/**
 * Executes a SQL query and returns the first matching row or null.
 * @param databaseName - The connection ID
 * @param sql - SQL query to execute
 * @param params - Parameters for the SQL query
 * @returns Single row object or null
 */
export async function querySingle<T = any>(
    databaseName: string,
    sql: string,
    params: any[] = []
): Promise<T | null> {
    const results = await query<T>(databaseName, sql, params);
    return results.length > 0 ? results[0] : null;
}

/**
 * Executes multiple SQL statements in a batch.
 * @param databaseName - The connection ID
 * @param statements - Array of SQL statements with their parameters
 * @returns Array of execution results
 */
export async function executeBatch(
    databaseName: string,
    statements: Array<{ sql: string; params: any[] }>
): Promise<ExecuteResult[]> {
    const conn = getConnection(databaseName);
    const batch = statements.map(s => ({ sql: s.sql, args: s.params }));
    const results = await conn.client.batch(batch);

    return results.map(r => ({
        rowsAffected: r.rowsAffected,
        lastInsertRowId: Number(r.lastInsertRowid ?? 0)
    }));
}

// ============================================================================
// Prepared Statements
// ============================================================================

/**
 * Creates a prepared statement for efficient repeated execution.
 * @param databaseName - The connection ID
 * @param sql - SQL statement to prepare
 * @returns Statement ID for use with statement* functions
 */
export function prepare(
    databaseName: string,
    sql: string
): string {
    const conn = getConnection(databaseName);
    const stmtId = `stmt_${++statementCounter}`;

    // Store the SQL template - libsql/client doesn't have true prepared statements
    // but we can cache the SQL for convenience
    conn.statements.set(stmtId, { sql, args: [] });

    return stmtId;
}

/**
 * Executes a prepared statement with the given parameters.
 * @param databaseName - The connection ID
 * @param statementId - The prepared statement ID
 * @param params - Parameters for the statement
 * @returns Execution result
 */
export async function statementRun(
    databaseName: string,
    statementId: string,
    params: any[] = []
): Promise<ExecuteResult> {
    const stmt = getStatement(databaseName, statementId);
    return execute(databaseName, stmt.sql as string, params);
}

/**
 * Executes a prepared query statement and returns all rows.
 * @param databaseName - The connection ID
 * @param statementId - The prepared statement ID
 * @param params - Parameters for the query
 * @returns Array of row objects
 */
export async function statementQuery<T = any>(
    databaseName: string,
    statementId: string,
    params: any[] = []
): Promise<T[]> {
    const stmt = getStatement(databaseName, statementId);
    return query<T>(databaseName, stmt.sql as string, params);
}

/**
 * Executes a prepared query statement and returns the first row.
 * @param databaseName - The connection ID
 * @param statementId - The prepared statement ID
 * @param params - Parameters for the query
 * @returns Single row object or null
 */
export async function statementQuerySingle<T = any>(
    databaseName: string,
    statementId: string,
    params: any[] = []
): Promise<T | null> {
    const stmt = getStatement(databaseName, statementId);
    return querySingle<T>(databaseName, stmt.sql as string, params);
}

/**
 * Finalizes a prepared statement and releases its resources.
 * @param databaseName - The connection ID
 * @param statementId - The prepared statement ID to finalize
 */
export function statementFinalize(
    databaseName: string,
    statementId: string
): void {
    const conn = connections.get(databaseName);
    if (conn) {
        conn.statements.delete(statementId);
    }
}

// ============================================================================
// Transactions
// ============================================================================

/**
 * Begins a new transaction.
 * @param databaseName - The connection ID
 * @returns Transaction ID (currently just returns a marker)
 */
export async function beginTransaction(databaseName: string): Promise<string> {
    await execute(databaseName, 'BEGIN TRANSACTION');
    return `txn_${++statementCounter}`;
}

/**
 * Commits the current transaction.
 * @param databaseName - The connection ID
 * @param transactionId - The transaction ID (unused, for API consistency)
 */
export async function commitTransaction(
    databaseName: string,
    _transactionId: string
): Promise<void> {
    await execute(databaseName, 'COMMIT');
}

/**
 * Rolls back the current transaction.
 * @param databaseName - The connection ID
 * @param transactionId - The transaction ID (unused, for API consistency)
 */
export async function rollbackTransaction(
    databaseName: string,
    _transactionId: string
): Promise<void> {
    await execute(databaseName, 'ROLLBACK');
}

// ============================================================================
// Sync Operations (for Turso Cloud)
// ============================================================================

/**
 * Pulls changes from the remote database.
 * Note: This requires a sync-enabled connection.
 * @param databaseName - The connection ID
 * @returns Sync result with timing and frame count
 */
export async function pull(databaseName: string): Promise<SyncResult> {
    const conn = getConnection(databaseName);

    if (!conn.isSyncEnabled) {
        throw new Error('Sync is not enabled for this connection. Use connectWithSync() to enable sync.');
    }

    const startTime = performance.now();

    try {
        // @ts-ignore - sync method may not be available on all client types
        if (typeof conn.client.sync === 'function') {
            // @ts-ignore
            const result = await conn.client.sync();
            const durationMs = Math.round(performance.now() - startTime);

            return {
                framesSynced: result?.frames?.count ?? 0,
                durationMs,
                success: true
            };
        } else {
            throw new Error('Sync is not supported for this connection type.');
        }
    } catch (error) {
        const durationMs = Math.round(performance.now() - startTime);
        return {
            framesSynced: 0,
            durationMs,
            success: false,
            errorMessage: error instanceof Error ? error.message : String(error)
        };
    }
}

/**
 * Pushes local changes to the remote database.
 * Note: For libsql/client, push is the same as sync.
 * @param databaseName - The connection ID
 * @returns Sync result with timing and frame count
 */
export async function push(databaseName: string): Promise<SyncResult> {
    // In libsql, sync is bidirectional, so push is the same as sync
    return await pull(databaseName);
}

/**
 * Performs bidirectional sync with the remote database.
 * @param databaseName - The connection ID
 * @returns Sync result with timing and frame count
 */
export async function sync(databaseName: string): Promise<SyncResult> {
    return await pull(databaseName);
}

// ============================================================================
// Helper Functions
// ============================================================================

/**
 * Gets a connection or throws if not found.
 */
function getConnection(databaseName: string): ConnectionInfo {
    const conn = connections.get(databaseName);
    if (!conn) {
        throw new Error(`Database connection not found: ${databaseName}. Call connect() first.`);
    }
    return conn;
}

/**
 * Gets a prepared statement or throws if not found.
 */
function getStatement(databaseName: string, statementId: string): InStatement {
    const conn = getConnection(databaseName);
    const stmt = conn.statements.get(statementId);
    if (!stmt) {
        throw new Error(`Statement not found: ${statementId}`);
    }
    return stmt;
}

/**
 * Converts a ResultSet to an array of typed objects.
 */
function rowsToObjects<T>(result: ResultSet): T[] {
    const columns = result.columns;
    return result.rows.map(row => {
        const obj: any = {};
        columns.forEach((col, i) => {
            obj[col] = row[i];
        });
        return obj as T;
    });
}
