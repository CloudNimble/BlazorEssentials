using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Base class for sync-enabled Turso databases. Extends <see cref="TursoDatabase"/>
    /// with Turso Cloud synchronization capabilities (pull, push, sync).
    /// </summary>
    /// <example>
    /// <code>
    /// public class AppDatabase : TursoSyncDatabase
    /// {
    ///     public TursoDbSet&lt;User&gt; Users { get; }
    ///
    ///     public AppDatabase(IJSRuntime jsRuntime) : base(jsRuntime)
    ///     {
    ///         Name = "app.db";
    ///         SyncUrl = "libsql://mydb-myorg.turso.io";
    ///         SyncAuthToken = "your-auth-token";
    ///         Users = new TursoDbSet&lt;User&gt;(this);
    ///     }
    /// }
    ///
    /// // Usage
    /// await Database.ConnectAsync();
    /// await Database.PullAsync();   // Pull changes from cloud
    /// // ... make local changes ...
    /// await Database.PushAsync();   // Push local changes to cloud
    /// </code>
    /// </example>
    public abstract class TursoSyncDatabase : TursoDatabase
    {

        #region Private Members

        private bool _isSyncing;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the sync URL for Turso Cloud.
        /// This is typically in the format: libsql://[database-name]-[org-name].turso.io
        /// </summary>
        public string SyncUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the authentication token for Turso Cloud sync.
        /// </summary>
        public string SyncAuthToken { get; set; } = "";

        /// <summary>
        /// Gets or sets whether to sync on connect.
        /// Default is true.
        /// </summary>
        public bool SyncOnConnect { get; set; } = true;

        /// <summary>
        /// Gets or sets the sync interval in milliseconds.
        /// Set to 0 to disable automatic periodic sync. Default is 0.
        /// </summary>
        public int SyncIntervalMs { get; set; } = 0;

        /// <summary>
        /// Gets whether a sync operation is currently in progress.
        /// </summary>
        public bool IsSyncing => _isSyncing;

        /// <summary>
        /// Gets or sets whether encryption is enabled for the local database.
        /// </summary>
        public bool EncryptionEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the encryption key for the local database.
        /// </summary>
        public string? EncryptionKey { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a sync operation starts.
        /// </summary>
        public event EventHandler? SyncStarted;

        /// <summary>
        /// Occurs when a sync operation completes successfully.
        /// </summary>
        public event EventHandler<TursoSyncResult>? SyncCompleted;

        /// <summary>
        /// Occurs when a sync operation fails.
        /// </summary>
        public event EventHandler<Exception>? SyncFailed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoSyncDatabase"/> class.
        /// </summary>
        /// <param name="jsRuntime">The JavaScript runtime for interop.</param>
        protected TursoSyncDatabase(IJSRuntime jsRuntime) : base(jsRuntime)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connects to the database with sync support.
        /// If <see cref="SyncOnConnect"/> is true, performs an initial sync after connecting.
        /// </summary>
        public new async Task ConnectAsync()
        {
            // Connect with sync URL and auth token
            await CallJavaScriptAsync<string>(
                TursoDbConstants.ConnectWithSync,
                Name,
                Url,
                AuthToken,
                SyncUrl,
                SyncAuthToken,
                EncryptionEnabled ? EncryptionKey : null);

            if (AutoCreateSchema)
            {
                // Schema creation is handled in base class, but we need to call it explicitly
                // since we're not calling base.ConnectAsync()
                foreach (var dbSet in DbSets)
                {
                    var metadata = dbSet.GetEntityMetadata();
                    var ddlStatements = Schema.SqlGenerator.GenerateAllDdl(metadata);

                    foreach (var ddl in ddlStatements)
                    {
                        await ExecuteAsync(ddl);
                    }
                }
            }

            if (SyncOnConnect && !string.IsNullOrEmpty(SyncUrl))
            {
                await SyncAsync();
            }
        }

        /// <summary>
        /// Pulls changes from Turso Cloud to the local database.
        /// This downloads any changes made on other devices or the cloud.
        /// </summary>
        /// <returns>The sync result containing the number of frames synced.</returns>
        public async Task<TursoSyncResult> PullAsync()
        {
            await EnsureConnectedAsync();
            ValidateSyncConfiguration();

            try
            {
                _isSyncing = true;
                SyncStarted?.Invoke(this, EventArgs.Empty);

                var result = await CallJavaScriptAsync<TursoSyncResult>(TursoDbConstants.Pull, Name);

                SyncCompleted?.Invoke(this, result);
                return result;
            }
            catch (Exception ex)
            {
                SyncFailed?.Invoke(this, ex);
                throw;
            }
            finally
            {
                _isSyncing = false;
            }
        }

        /// <summary>
        /// Pushes local changes to Turso Cloud.
        /// This uploads any local changes to the cloud for other devices to sync.
        /// </summary>
        /// <returns>The sync result containing the number of frames synced.</returns>
        public async Task<TursoSyncResult> PushAsync()
        {
            await EnsureConnectedAsync();
            ValidateSyncConfiguration();

            try
            {
                _isSyncing = true;
                SyncStarted?.Invoke(this, EventArgs.Empty);

                var result = await CallJavaScriptAsync<TursoSyncResult>(TursoDbConstants.Push, Name);

                SyncCompleted?.Invoke(this, result);
                return result;
            }
            catch (Exception ex)
            {
                SyncFailed?.Invoke(this, ex);
                throw;
            }
            finally
            {
                _isSyncing = false;
            }
        }

        /// <summary>
        /// Performs a bidirectional sync with Turso Cloud.
        /// This combines pull and push operations.
        /// </summary>
        /// <returns>The sync result containing the number of frames synced.</returns>
        public async Task<TursoSyncResult> SyncAsync()
        {
            await EnsureConnectedAsync();
            ValidateSyncConfiguration();

            try
            {
                _isSyncing = true;
                SyncStarted?.Invoke(this, EventArgs.Empty);

                var result = await CallJavaScriptAsync<TursoSyncResult>(TursoDbConstants.Sync, Name);

                SyncCompleted?.Invoke(this, result);
                return result;
            }
            catch (Exception ex)
            {
                SyncFailed?.Invoke(this, ex);
                throw;
            }
            finally
            {
                _isSyncing = false;
            }
        }

        #endregion

        #region Private Methods

        private void ValidateSyncConfiguration()
        {
            if (string.IsNullOrWhiteSpace(SyncUrl))
            {
                throw new InvalidOperationException("SyncUrl must be configured for sync operations.");
            }

            if (string.IsNullOrWhiteSpace(SyncAuthToken))
            {
                throw new InvalidOperationException("SyncAuthToken must be configured for sync operations.");
            }
        }

        #endregion

    }

}
