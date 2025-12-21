namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Contains constant strings for JavaScript interop function names.
    /// </summary>
    internal static class TursoDbConstants
    {
        // Database lifecycle
        internal const string Connect = "connect";
        internal const string ConnectWithSync = "connectWithSync";
        internal const string Disconnect = "disconnect";
        internal const string IsConnected = "isConnected";

        // SQL execution
        internal const string Execute = "execute";
        internal const string Query = "query";
        internal const string QuerySingle = "querySingle";
        internal const string ExecuteBatch = "executeBatch";

        // Prepared statements
        internal const string Prepare = "prepare";
        internal const string StatementRun = "statementRun";
        internal const string StatementQuery = "statementQuery";
        internal const string StatementQuerySingle = "statementQuerySingle";
        internal const string StatementFinalize = "statementFinalize";

        // Transactions
        internal const string BeginTransaction = "beginTransaction";
        internal const string CommitTransaction = "commitTransaction";
        internal const string RollbackTransaction = "rollbackTransaction";

        // Sync operations (for TursoSyncDatabase)
        internal const string Pull = "pull";
        internal const string Push = "push";
        internal const string Sync = "sync";
    }
}
