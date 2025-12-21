using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Marks a property as the primary key of the table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets whether the primary key auto-increments.
        /// Defaults to true for integer types.
        /// </summary>
        public bool AutoIncrement { get; set; } = true;

    }

}
