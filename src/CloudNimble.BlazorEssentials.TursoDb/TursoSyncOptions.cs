namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Configuration options for Turso Cloud sync.
    /// </summary>
    public class TursoSyncOptions
    {

        /// <summary>
        /// Gets or sets the sync URL for the Turso Cloud database.
        /// This is typically in the format: libsql://[database-name]-[org-name].turso.io
        /// </summary>
        public string SyncUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the authentication token for Turso Cloud.
        /// This token is obtained from the Turso CLI or dashboard.
        /// </summary>
        public string AuthToken { get; set; } = "";

        /// <summary>
        /// Gets or sets the sync interval in milliseconds.
        /// Set to 0 to disable automatic sync. Default is 0 (manual sync only).
        /// </summary>
        public int SyncIntervalMs { get; set; } = 0;

        /// <summary>
        /// Gets or sets whether to automatically sync on connect.
        /// Default is true.
        /// </summary>
        public bool SyncOnConnect { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to encrypt the local database.
        /// Requires an encryption key to be set.
        /// </summary>
        public bool EncryptionEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the encryption key for the local database.
        /// Required when EncryptionEnabled is true.
        /// </summary>
        public string? EncryptionKey { get; set; }

    }

}
