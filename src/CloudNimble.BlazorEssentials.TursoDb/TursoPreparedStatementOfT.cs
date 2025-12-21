using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents a prepared SQL statement that can be executed multiple times
    /// with different parameters for improved performance.
    /// </summary>
    /// <example>
    /// <code>
    /// var statement = Database.Prepare&lt;User&gt;("SELECT * FROM users WHERE age > ?");
    /// var youngUsers = await statement.QueryAsync(18);
    /// var olderUsers = await statement.QueryAsync(65);
    /// </code>
    /// </example>
    /// <typeparam name="TResult">The type of result returned by queries.</typeparam>
    public class TursoPreparedStatement<TResult> : IAsyncDisposable where TResult : class, new()
    {

        #region Private Members

        private readonly TursoDatabase _database;
        private readonly string _sql;
        private readonly string _statementId;
        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the SQL statement.
        /// </summary>
        public string Sql => _sql;

        /// <summary>
        /// Gets the unique identifier for this prepared statement.
        /// </summary>
        public string StatementId => _statementId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoPreparedStatement{TResult}"/> class.
        /// </summary>
        /// <param name="database">The database this statement belongs to.</param>
        /// <param name="sql">The SQL statement to prepare.</param>
        internal TursoPreparedStatement(TursoDatabase database, string sql)
        {
            ArgumentNullException.ThrowIfNull(database);
            ArgumentException.ThrowIfNullOrWhiteSpace(sql);

            _database = database;
            _sql = sql;
            _statementId = Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the prepared statement as a query and returns all matching rows.
        /// </summary>
        /// <param name="parameters">The parameters for the statement.</param>
        /// <returns>A list of matching results.</returns>
        public async Task<List<TResult>> QueryAsync(params object?[] parameters)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            await _database.EnsureConnectedAsync();
            return await _database.QueryAsync<TResult>(_sql, parameters);
        }

        /// <summary>
        /// Executes the prepared statement as a query and returns the first matching row or null.
        /// </summary>
        /// <param name="parameters">The parameters for the statement.</param>
        /// <returns>The first matching result or null.</returns>
        public async Task<TResult?> QuerySingleAsync(params object?[] parameters)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            await _database.EnsureConnectedAsync();
            return await _database.QuerySingleAsync<TResult>(_sql, parameters);
        }

        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            if (_disposed) return ValueTask.CompletedTask;

            _disposed = true;
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        #endregion

    }

}
