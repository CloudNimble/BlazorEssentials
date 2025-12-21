using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Creates an index on the specified column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class IndexAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets whether the index enforces uniqueness.
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Gets or sets the index name.
        /// If not specified, the name is auto-generated as "ix_{tablename}_{columnname}".
        /// </summary>
        public string? Name { get; set; }

    }

}
