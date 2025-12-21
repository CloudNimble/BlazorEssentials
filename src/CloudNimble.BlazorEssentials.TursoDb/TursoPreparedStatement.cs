using System;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents a prepared SQL statement for executing non-query commands.
    /// </summary>
    public class TursoPreparedStatement : IAsyncDisposable
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
        /// Initializes a new instance of the <see cref="TursoPreparedStatement"/> class.
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
        /// Executes the prepared statement and returns the result.
        /// </summary>
        /// <param name="parameters">The parameters for the statement.</param>
        /// <returns>The execution result.</returns>
        public async Task<TursoResult> ExecuteAsync(params object?[] parameters)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            await _database.EnsureConnectedAsync();
            return await _database.ExecuteAsync(_sql, parameters);
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
