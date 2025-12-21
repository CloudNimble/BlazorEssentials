using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Specifies the column name and optional type for a property.
    /// If not specified, the property name is used as the column name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ColumnAttribute : Attribute
    {

        /// <summary>
        /// Gets the column name in the database.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the SQLite column type (e.g., "TEXT", "INTEGER", "REAL", "BLOB").
        /// If not specified, the type is inferred from the property type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets whether the column allows NULL values.
        /// If not specified, nullability is inferred from the property type.
        /// </summary>
        public bool? Nullable { get; set; }

        /// <summary>
        /// Gets or sets the default value for the column.
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        /// <param name="name">The column name in the database.</param>
        /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
        public ColumnAttribute(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
        }

    }

}
