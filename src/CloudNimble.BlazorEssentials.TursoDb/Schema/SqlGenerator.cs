using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudNimble.BlazorEssentials.TursoDb.Schema
{

    /// <summary>
    /// Generates SQL DDL statements from entity metadata.
    /// </summary>
    public static class SqlGenerator
    {

        /// <summary>
        /// Generates a CREATE TABLE IF NOT EXISTS statement for the entity.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL CREATE TABLE statement.</returns>
        public static string GenerateCreateTable(EntityMetadata metadata)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE IF NOT EXISTS {metadata.TableName} (");

            var columnDefinitions = new List<string>();

            foreach (var column in metadata.Columns)
            {
                var def = new StringBuilder();
                def.Append($"    {column.ColumnName} {column.SqliteType}");

                if (column.IsPrimaryKey)
                {
                    def.Append(" PRIMARY KEY");
                    if (column.AutoIncrement)
                    {
                        def.Append(" AUTOINCREMENT");
                    }
                }

                if (!column.IsPrimaryKey && !column.IsNullable)
                {
                    def.Append(" NOT NULL");
                }

                if (column.DefaultValue is not null)
                {
                    def.Append($" DEFAULT {column.DefaultValue}");
                }

                columnDefinitions.Add(def.ToString());
            }

            sb.AppendLine(string.Join(",\n", columnDefinitions));
            sb.Append(')');

            return sb.ToString();
        }

        /// <summary>
        /// Generates CREATE INDEX statements for all indexed columns.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL CREATE INDEX statements.</returns>
        public static IEnumerable<string> GenerateCreateIndexes(EntityMetadata metadata)
        {
            foreach (var column in metadata.Columns.Where(c => c.HasIndex && !c.IsPrimaryKey))
            {
                var unique = column.IsUniqueIndex ? "UNIQUE " : "";
                yield return $"CREATE {unique}INDEX IF NOT EXISTS {column.IndexName} ON {metadata.TableName} ({column.ColumnName})";
            }
        }

        /// <summary>
        /// Generates all DDL statements needed to create the table and indexes.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>All SQL DDL statements.</returns>
        public static IEnumerable<string> GenerateAllDdl(EntityMetadata metadata)
        {
            yield return GenerateCreateTable(metadata);

            foreach (var index in GenerateCreateIndexes(metadata))
            {
                yield return index;
            }
        }

        /// <summary>
        /// Generates an INSERT statement for the entity.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <param name="columns">The column names.</param>
        /// <param name="placeholders">The parameter placeholders.</param>
        /// <returns>The SQL INSERT statement.</returns>
        public static string GenerateInsert(EntityMetadata metadata, string columns, string placeholders)
        {
            return $"INSERT INTO {metadata.TableName} ({columns}) VALUES ({placeholders})";
        }

        /// <summary>
        /// Generates an UPDATE statement for the entity.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <param name="setClause">The SET clause.</param>
        /// <returns>The SQL UPDATE statement.</returns>
        public static string GenerateUpdate(EntityMetadata metadata, string setClause)
        {
            if (metadata.PrimaryKey is null)
            {
                throw new InvalidOperationException($"Cannot generate UPDATE for {metadata.TableName} without a primary key.");
            }

            return $"UPDATE {metadata.TableName} SET {setClause} WHERE {metadata.PrimaryKey.ColumnName} = ?";
        }

        /// <summary>
        /// Generates a DELETE statement for the entity.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL DELETE statement.</returns>
        public static string GenerateDelete(EntityMetadata metadata)
        {
            if (metadata.PrimaryKey is null)
            {
                throw new InvalidOperationException($"Cannot generate DELETE for {metadata.TableName} without a primary key.");
            }

            return $"DELETE FROM {metadata.TableName} WHERE {metadata.PrimaryKey.ColumnName} = ?";
        }

        /// <summary>
        /// Generates a SELECT * statement for the entity.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL SELECT statement.</returns>
        public static string GenerateSelectAll(EntityMetadata metadata)
        {
            return $"SELECT * FROM {metadata.TableName}";
        }

        /// <summary>
        /// Generates a SELECT statement to find by primary key.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL SELECT statement.</returns>
        public static string GenerateSelectByKey(EntityMetadata metadata)
        {
            if (metadata.PrimaryKey is null)
            {
                throw new InvalidOperationException($"Cannot generate SELECT by key for {metadata.TableName} without a primary key.");
            }

            return $"SELECT * FROM {metadata.TableName} WHERE {metadata.PrimaryKey.ColumnName} = ? LIMIT 1";
        }

        /// <summary>
        /// Generates a SELECT COUNT(*) statement.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <returns>The SQL COUNT statement.</returns>
        public static string GenerateCount(EntityMetadata metadata)
        {
            return $"SELECT COUNT(*) FROM {metadata.TableName}";
        }

    }

}
