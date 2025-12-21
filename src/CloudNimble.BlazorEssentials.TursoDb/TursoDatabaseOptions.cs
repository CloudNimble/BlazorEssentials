namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Configuration options for a Turso database.
    /// </summary>
    public class TursoDatabaseOptions
    {

        /// <summary>
        /// Gets or sets the database URL.
        /// Use "file:name.db" for local databases, or a Turso cloud URL for remote.
        /// Defaults to ":memory:" for an in-memory database.
        /// </summary>
        public string Url { get; set; } = ":memory:";

        /// <summary>
        /// Gets or sets the authentication token for remote Turso databases.
        /// </summary>
        public string? AuthToken { get; set; }

        /// <summary>
        /// Gets or sets whether to automatically create tables on connect.
        /// Defaults to true.
        /// </summary>
        public bool AutoCreateSchema { get; set; } = true;

    }

}
