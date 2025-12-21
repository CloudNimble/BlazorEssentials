using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Specifies the table name for an entity class.
    /// If not specified, the class name is used as the table name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TableAttribute : Attribute
    {

        /// <summary>
        /// Gets the table name in the database.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        /// <param name="name">The table name in the database.</param>
        /// <exception cref="ArgumentException">Thrown when name is null or whitespace.</exception>
        public TableAttribute(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
        }

    }

}
