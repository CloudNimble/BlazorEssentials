using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CloudNimble.BlazorEssentials.TursoDb.Schema
{

    /// <summary>
    /// Contains metadata about an entity type for database operations.
    /// </summary>
    public class EntityMetadata
    {

        #region Properties

        /// <summary>
        /// Gets the entity type.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Gets the table name in the database.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the list of columns in the entity.
        /// </summary>
        public IReadOnlyList<ColumnMetadata> Columns { get; }

        /// <summary>
        /// Gets the primary key column, or null if none.
        /// </summary>
        public ColumnMetadata? PrimaryKey { get; }

        /// <summary>
        /// Gets the columns that are not the primary key.
        /// </summary>
        public IReadOnlyList<ColumnMetadata> NonKeyColumns { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMetadata"/> class.
        /// </summary>
        /// <param name="entityType">The entity type to analyze.</param>
        public EntityMetadata(Type entityType)
        {
            EntityType = entityType;
            TableName = GetTableName(entityType);
            Columns = BuildColumns(entityType).ToList();
            PrimaryKey = Columns.FirstOrDefault(c => c.IsPrimaryKey);
            NonKeyColumns = Columns.Where(c => !c.IsPrimaryKey).ToList();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the primary key value from an entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns>The primary key value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no primary key is defined.</exception>
        public object? GetPrimaryKeyValue(object entity)
        {
            if (PrimaryKey is null)
            {
                throw new InvalidOperationException($"Entity type {EntityType.Name} does not have a primary key defined.");
            }
            return PrimaryKey.GetValue(entity);
        }

        /// <summary>
        /// Sets the primary key value on an entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="value">The primary key value.</param>
        /// <exception cref="InvalidOperationException">Thrown when no primary key is defined.</exception>
        public void SetPrimaryKeyValue(object entity, object? value)
        {
            if (PrimaryKey is null)
            {
                throw new InvalidOperationException($"Entity type {EntityType.Name} does not have a primary key defined.");
            }
            PrimaryKey.SetValue(entity, value);
        }

        /// <summary>
        /// Gets the column values for insert, excluding auto-increment primary keys with default values.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns>Column names, placeholders, and parameter values.</returns>
        public (string columns, string placeholders, object?[] parameters) GetInsertData(object entity)
        {
            var columnsToInsert = Columns
                .Where(c => !(c.IsPrimaryKey && c.AutoIncrement && IsDefaultValue(c.GetValue(entity), c.Property.PropertyType)))
                .ToList();

            var columnNames = string.Join(", ", columnsToInsert.Select(c => c.ColumnName));
            var placeholders = string.Join(", ", columnsToInsert.Select(_ => "?"));
            var parameters = columnsToInsert.Select(c => ConvertToSqlValue(c.GetValue(entity))).ToArray();

            return (columnNames, placeholders, parameters);
        }

        /// <summary>
        /// Gets the column values for update, excluding the primary key.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns>SET clause and parameter values including the primary key.</returns>
        public (string setClause, object?[] parameters) GetUpdateData(object entity)
        {
            var setClause = string.Join(", ", NonKeyColumns.Select(c => $"{c.ColumnName} = ?"));
            var parameters = NonKeyColumns
                .Select(c => ConvertToSqlValue(c.GetValue(entity)))
                .Append(ConvertToSqlValue(GetPrimaryKeyValue(entity)))
                .ToArray();

            return (setClause, parameters);
        }

        #endregion

        #region Private Methods

        private static string GetTableName(Type entityType)
        {
            var tableAttr = entityType.GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? entityType.Name;
        }

        private IEnumerable<ColumnMetadata> BuildColumns(Type entityType)
        {
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>() is null);

            foreach (var property in properties)
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                var primaryKeyAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
                var indexAttr = property.GetCustomAttribute<IndexAttribute>();

                var columnName = columnAttr?.Name ?? property.Name;
                var sqliteType = columnAttr?.Type ?? GetSqliteType(property.PropertyType);
                var isPrimaryKey = primaryKeyAttr is not null;
                var autoIncrement = primaryKeyAttr?.AutoIncrement ?? (isPrimaryKey && IsIntegerType(property.PropertyType));
                var isNullable = columnAttr?.Nullable ?? IsNullableType(property.PropertyType);
                var defaultValue = columnAttr?.DefaultValue;
                var hasIndex = indexAttr is not null;
                var isUniqueIndex = indexAttr?.Unique ?? false;
                var indexName = indexAttr?.Name ?? (hasIndex ? $"ix_{TableName}_{columnName}" : null);

                yield return new ColumnMetadata(
                    property,
                    columnName,
                    sqliteType,
                    isPrimaryKey,
                    autoIncrement,
                    isNullable,
                    defaultValue,
                    hasIndex,
                    isUniqueIndex,
                    indexName);
            }
        }

        private static string GetSqliteType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return underlyingType switch
            {
                Type t when t == typeof(int) => "INTEGER",
                Type t when t == typeof(long) => "INTEGER",
                Type t when t == typeof(short) => "INTEGER",
                Type t when t == typeof(byte) => "INTEGER",
                Type t when t == typeof(bool) => "INTEGER",
                Type t when t == typeof(float) => "REAL",
                Type t when t == typeof(double) => "REAL",
                Type t when t == typeof(decimal) => "REAL",
                Type t when t == typeof(DateTime) => "TEXT",
                Type t when t == typeof(DateTimeOffset) => "TEXT",
                Type t when t == typeof(Guid) => "TEXT",
                Type t when t == typeof(byte[]) => "BLOB",
                _ => "TEXT"
            };
        }

        private static bool IsIntegerType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType == typeof(int)
                || underlyingType == typeof(long)
                || underlyingType == typeof(short)
                || underlyingType == typeof(byte);
        }

        private static bool IsNullableType(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) is not null;
        }

        private static bool IsDefaultValue(object? value, Type type)
        {
            if (value is null) return true;

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            var defaultValue = underlyingType.IsValueType ? Activator.CreateInstance(underlyingType) : null;

            return Equals(value, defaultValue);
        }

        private static object? ConvertToSqlValue(object? value)
        {
            return value switch
            {
                null => null,
                DateTime dt => dt.ToString("O"),
                DateTimeOffset dto => dto.ToString("O"),
                Guid g => g.ToString(),
                bool b => b ? 1 : 0,
                _ => value
            };
        }

        #endregion

    }

}
