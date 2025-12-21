using CloudNimble.BlazorEssentials.TursoDb.Schema;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Base class for Turso databases. Inherit from this class and add
    /// <see cref="TursoDbSet{TEntity}"/> properties for each entity type.
    /// </summary>
    /// <example>
    /// <code>
    /// public class AppDatabase : TursoDatabase
    /// {
    ///     public TursoDbSet&lt;User&gt; Users { get; }
    ///     public TursoDbSet&lt;Post&gt; Posts { get; }
    ///
    ///     public AppDatabase(IJSRuntime jsRuntime) : base(jsRuntime)
    ///     {
    ///         Name = "app.db";
    ///         Users = new TursoDbSet&lt;User&gt;(this);
    ///         Posts = new TursoDbSet&lt;Post&gt;(this);
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class TursoDatabase : IAsyncDisposable
    {

        #region Private Members

        private bool _isConnected;
        private bool _disposed;
        private readonly IJSRuntime _jsRuntime;
        private readonly JsModule _tursoModule;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the database name/identifier.
        /// This is used as the connection key in JavaScript.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the database URL.
        /// Use ":memory:" for in-memory, "file:name.db" for local file, or a Turso cloud URL.
        /// </summary>
        public string Url { get; set; } = ":memory:";

        /// <summary>
        /// Gets or sets the authentication token for remote Turso databases.
        /// </summary>
        public string? AuthToken { get; set; }

        /// <summary>
        /// Gets or sets whether to automatically create tables on connect.
        /// </summary>
        public bool AutoCreateSchema { get; set; } = true;

        /// <summary>
        /// Gets the list of DbSets in this database.
        /// </summary>
        public List<ITursoDbSet> DbSets { get; } = [];

        /// <summary>
        /// Gets whether the database is currently connected.
        /// </summary>
        public bool IsConnected => _isConnected;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoDatabase"/> class.
        /// </summary>
        /// <param name="jsRuntime">The JavaScript runtime for interop.</param>
        /// <exception cref="ArgumentNullException">Thrown when jsRuntime is null.</exception>
        protected TursoDatabase(IJSRuntime jsRuntime)
        {
            ArgumentNullException.ThrowIfNull(jsRuntime);

            _jsRuntime = jsRuntime;
            var assemblyName = typeof(TursoDatabase).Assembly.GetName().Name;
            _tursoModule = new JsModule(jsRuntime, assemblyName![(assemblyName!.IndexOf('.') + 1)..], assemblyName);
            Name = GetType().Name;

            // Discover and initialize TursoDbSet properties
            DiscoverDbSets();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connects to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ConnectAsync()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_isConnected)
            {
                return;
            }

            await CallJavaScriptAsync<string>(TursoDbConstants.Connect, Name, Url, AuthToken);
            _isConnected = true;

            if (AutoCreateSchema)
            {
                await CreateSchemaAsync();
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DisconnectAsync()
        {
            if (!_isConnected) return;

            await CallJavaScriptAsync(TursoDbConstants.Disconnect, Name);
            _isConnected = false;
        }

        /// <summary>
        /// Executes a SQL statement and returns the result.
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <param name="parameters">The parameters for the SQL statement.</param>
        /// <returns>The execution result.</returns>
        public async Task<TursoResult> ExecuteAsync(string sql, params object?[] parameters)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            await EnsureConnectedAsync();

            return await CallJavaScriptAsync<TursoResult>(TursoDbConstants.Execute, Name, sql, parameters);
        }

        /// <summary>
        /// Executes a SQL query and returns all matching rows.
        /// </summary>
        /// <typeparam name="T">The type to deserialize results into.</typeparam>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the SQL query.</param>
        /// <returns>A list of results.</returns>
        public async Task<List<T>> QueryAsync<T>(string sql, params object?[] parameters)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            await EnsureConnectedAsync();

            return await CallJavaScriptAsync<List<T>>(TursoDbConstants.Query, Name, sql, parameters);
        }

        /// <summary>
        /// Executes a SQL query and returns the first matching row or null.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the result into.</typeparam>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the SQL query.</param>
        /// <returns>The first result or default.</returns>
        public async Task<T?> QuerySingleAsync<T>(string sql, params object?[] parameters)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            await EnsureConnectedAsync();

            return await CallJavaScriptAsync<T?>(TursoDbConstants.QuerySingle, Name, sql, parameters);
        }

        /// <summary>
        /// Executes multiple SQL statements in a batch.
        /// </summary>
        /// <param name="statements">The statements to execute.</param>
        /// <returns>The results of each statement.</returns>
        public async Task<List<TursoResult>> ExecuteBatchAsync(params (string sql, object?[] parameters)[] statements)
        {
            await EnsureConnectedAsync();

            var batch = statements.Select(s => new { sql = s.sql, @params = s.parameters }).ToArray();
            return await CallJavaScriptAsync<List<TursoResult>>(TursoDbConstants.ExecuteBatch, Name, batch);
        }

        /// <summary>
        /// Ensures the database is connected, connecting if necessary.
        /// </summary>
        public async Task EnsureConnectedAsync()
        {
            if (!_isConnected)
            {
                await ConnectAsync();
            }
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>A transaction that can be committed or rolled back.</returns>
        /// <example>
        /// <code>
        /// await using var transaction = await Database.BeginTransactionAsync();
        /// try
        /// {
        ///     await Database.Users.AddAsync(user);
        ///     await transaction.CommitAsync();
        /// }
        /// catch
        /// {
        ///     await transaction.RollbackAsync();
        ///     throw;
        /// }
        /// </code>
        /// </example>
        public async Task<TursoTransaction> BeginTransactionAsync()
        {
            await EnsureConnectedAsync();
            var transaction = new TursoTransaction(this);
            await transaction.BeginAsync();
            return transaction;
        }

        /// <summary>
        /// Creates a prepared statement for queries that return results.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
        /// <param name="sql">The SQL query to prepare.</param>
        /// <returns>A prepared statement that can be executed multiple times.</returns>
        public TursoPreparedStatement<TResult> Prepare<TResult>(string sql) where TResult : class, new()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            return new TursoPreparedStatement<TResult>(this, sql);
        }

        /// <summary>
        /// Creates a prepared statement for non-query commands.
        /// </summary>
        /// <param name="sql">The SQL command to prepare.</param>
        /// <returns>A prepared statement that can be executed multiple times.</returns>
        public TursoPreparedStatement Prepare(string sql)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);
            return new TursoPreparedStatement(this, sql);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;

            if (_isConnected)
            {
                await DisconnectAsync();
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calls a JavaScript function in the Turso module.
        /// </summary>
        internal async Task CallJavaScriptAsync(string functionName, params object?[] args)
        {
            try
            {
                await _tursoModule.InvokeVoidAsync(functionName, args);
            }
            catch (JSException e)
            {
                throw new TursoDbException(e.Message);
            }
        }

        /// <summary>
        /// Calls a JavaScript function in the Turso module and returns the result.
        /// </summary>
        internal async Task<TResult> CallJavaScriptAsync<TResult>(string functionName, params object?[] args)
        {
            try
            {
                return await _tursoModule.InvokeAsync<TResult>(functionName, args);
            }
            catch (JSException e)
            {
                throw new TursoDbException(e.Message);
            }
        }

        #endregion

        #region Private Methods

        private void DiscoverDbSets()
        {
            var dbSetProperties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(ITursoDbSet).IsAssignableFrom(p.PropertyType));

            foreach (var prop in dbSetProperties)
            {
                var dbSet = prop.GetValue(this) as ITursoDbSet;
                if (dbSet is not null)
                {
                    DbSets.Add(dbSet);
                }
            }
        }

        private async Task CreateSchemaAsync()
        {
            foreach (var dbSet in DbSets)
            {
                var metadata = dbSet.GetEntityMetadata();
                var ddlStatements = SqlGenerator.GenerateAllDdl(metadata);

                foreach (var ddl in ddlStatements)
                {
                    await ExecuteAsync(ddl);
                }
            }
        }

        #endregion

    }

}
