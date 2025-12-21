using System;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents a database transaction that can be committed or rolled back.
    /// Implements IAsyncDisposable for automatic rollback if not committed.
    /// </summary>
    /// <example>
    /// <code>
    /// await using var transaction = await Database.BeginTransactionAsync();
    /// try
    /// {
    ///     await Database.Users.AddAsync(user);
    ///     await Database.Posts.AddAsync(post);
    ///     await transaction.CommitAsync();
    /// }
    /// catch
    /// {
    ///     await transaction.RollbackAsync();
    ///     throw;
    /// }
    /// </code>
    /// </example>
    public class TursoTransaction : IAsyncDisposable
    {

        #region Private Members

        private readonly TursoDatabase _database;
        private bool _isCompleted;
        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the database this transaction belongs to.
        /// </summary>
        public TursoDatabase Database => _database;

        /// <summary>
        /// Gets whether the transaction has been completed (committed or rolled back).
        /// </summary>
        public bool IsCompleted => _isCompleted;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoTransaction"/> class.
        /// </summary>
        /// <param name="database">The database to create the transaction for.</param>
        internal TursoTransaction(TursoDatabase database)
        {
            _database = database;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Begins the transaction by executing BEGIN TRANSACTION.
        /// </summary>
        internal async Task BeginAsync()
        {
            await _database.ExecuteAsync("BEGIN TRANSACTION");
        }

        /// <summary>
        /// Commits the transaction, making all changes permanent.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the transaction is already completed.</exception>
        public async Task CommitAsync()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_isCompleted)
            {
                throw new InvalidOperationException("Transaction has already been completed.");
            }

            await _database.ExecuteAsync("COMMIT");
            _isCompleted = true;
        }

        /// <summary>
        /// Rolls back the transaction, discarding all changes.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the transaction is already completed.</exception>
        public async Task RollbackAsync()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_isCompleted)
            {
                throw new InvalidOperationException("Transaction has already been completed.");
            }

            await _database.ExecuteAsync("ROLLBACK");
            _isCompleted = true;
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;

            // Auto-rollback if not committed
            if (!_isCompleted)
            {
                try
                {
                    await RollbackAsync();
                }
                catch
                {
                    // Ignore errors during auto-rollback on dispose
                }
            }

            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
