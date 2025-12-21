namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents the result of a SQL execution operation.
    /// </summary>
    public class TursoResult
    {

        /// <summary>
        /// Gets or sets the number of rows affected by the operation.
        /// </summary>
        public int RowsAffected { get; set; }

        /// <summary>
        /// Gets or sets the row ID of the last inserted row.
        /// </summary>
        public long LastInsertRowId { get; set; }

    }

}
