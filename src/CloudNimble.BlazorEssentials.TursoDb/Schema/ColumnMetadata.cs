using System;
using System.Reflection;

namespace CloudNimble.BlazorEssentials.TursoDb.Schema
{

    /// <summary>
    /// Contains metadata about a column in a table.
    /// </summary>
    public class ColumnMetadata
    {

        /// <summary>
        /// Gets the property info for this column.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Gets the column name in the database.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the SQLite type for this column.
        /// </summary>
        public string SqliteType { get; }

        /// <summary>
        /// Gets whether this column is the primary key.
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// Gets whether the primary key auto-increments.
        /// </summary>
        public bool AutoIncrement { get; }

        /// <summary>
        /// Gets whether the column allows NULL values.
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        /// Gets the default value for the column, or null if none.
        /// </summary>
        public string? DefaultValue { get; }

        /// <summary>
        /// Gets whether this column has an index.
        /// </summary>
        public bool HasIndex { get; }

        /// <summary>
        /// Gets whether the index is unique.
        /// </summary>
        public bool IsUniqueIndex { get; }

        /// <summary>
        /// Gets the index name, if any.
        /// </summary>
        public string? IndexName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnMetadata"/> class.
        /// </summary>
        public ColumnMetadata(
            PropertyInfo property,
            string columnName,
            string sqliteType,
            bool isPrimaryKey,
            bool autoIncrement,
            bool isNullable,
            string? defaultValue,
            bool hasIndex,
            bool isUniqueIndex,
            string? indexName)
        {
            Property = property;
            ColumnName = columnName;
            SqliteType = sqliteType;
            IsPrimaryKey = isPrimaryKey;
            AutoIncrement = autoIncrement;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
            HasIndex = hasIndex;
            IsUniqueIndex = isUniqueIndex;
            IndexName = indexName;
        }

        /// <summary>
        /// Gets the value of this column from an entity instance.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns>The column value.</returns>
        public object? GetValue(object entity)
        {
            return Property.GetValue(entity);
        }

        /// <summary>
        /// Sets the value of this column on an entity instance.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(object entity, object? value)
        {
            if (value is null && !IsNullable)
            {
                return;
            }

            var targetType = Nullable.GetUnderlyingType(Property.PropertyType) ?? Property.PropertyType;

            if (value is not null && value.GetType() != targetType)
            {
                value = Convert.ChangeType(value, targetType);
            }

            Property.SetValue(entity, value);
        }

    }

}
