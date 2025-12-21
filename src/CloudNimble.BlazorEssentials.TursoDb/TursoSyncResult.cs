namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents the result of a sync operation.
    /// </summary>
    public class TursoSyncResult
    {

        /// <summary>
        /// Gets or sets the number of frames synced.
        /// </summary>
        public int FramesSynced { get; set; }

        /// <summary>
        /// Gets or sets the duration of the sync operation in milliseconds.
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// Gets or sets whether the sync was successful.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets an error message if the sync failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

    }

}
