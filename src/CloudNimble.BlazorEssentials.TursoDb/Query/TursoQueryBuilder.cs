using CloudNimble.BlazorEssentials.TursoDb.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb.Query
{

    /// <summary>
    /// Provides fluent query building capabilities for TursoDbSet.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being queried.</typeparam>
    public class TursoQueryBuilder<TEntity> where TEntity : class, new()
    {

        #region Private Members

        private readonly TursoDatabase _database;
        private readonly EntityMetadata _metadata;
        private readonly List<(string clause, object?[] parameters)> _whereClauses = [];
        private readonly List<string> _orderByClauses = [];
        private int? _take;
        private int? _skip;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoQueryBuilder{TEntity}"/> class.
        /// </summary>
        /// <param name="database">The database to query.</param>
        internal TursoQueryBuilder(TursoDatabase database)
        {
            _database = database;
            _metadata = EntityMetadataCache.GetOrCreate<TEntity>();
        }

        #endregion

        #region Fluent Methods

        /// <summary>
        /// Adds a WHERE clause to the query.
        /// </summary>
        /// <param name="clause">The WHERE clause (e.g., "name = ?" or "age > ?").</param>
        /// <param name="parameters">The parameters for the clause.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> Where(string clause, params object?[] parameters)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clause);
            _whereClauses.Add((clause, parameters));
            return this;
        }

        /// <summary>
        /// Adds a WHERE clause for equality.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> WhereEquals(string column, object? value)
        {
            return Where($"{column} = ?", value);
        }

        /// <summary>
        /// Adds a WHERE clause for LIKE matching.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="pattern">The LIKE pattern (use % for wildcards).</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> WhereLike(string column, string pattern)
        {
            return Where($"{column} LIKE ?", pattern);
        }

        /// <summary>
        /// Adds a WHERE clause for NULL check.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> WhereNull(string column)
        {
            return Where($"{column} IS NULL");
        }

        /// <summary>
        /// Adds a WHERE clause for NOT NULL check.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> WhereNotNull(string column)
        {
            return Where($"{column} IS NOT NULL");
        }

        /// <summary>
        /// Adds a WHERE IN clause.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="values">The values to match.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> WhereIn(string column, params object[] values)
        {
            if (values.Length == 0) return this;

            var placeholders = string.Join(", ", Enumerable.Repeat("?", values.Length));
            return Where($"{column} IN ({placeholders})", values);
        }

        /// <summary>
        /// Adds an ORDER BY clause (ascending).
        /// </summary>
        /// <param name="column">The column to order by.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> OrderBy(string column)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(column);
            _orderByClauses.Add($"{column} ASC");
            return this;
        }

        /// <summary>
        /// Adds an ORDER BY clause (descending).
        /// </summary>
        /// <param name="column">The column to order by.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> OrderByDescending(string column)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(column);
            _orderByClauses.Add($"{column} DESC");
            return this;
        }

        /// <summary>
        /// Limits the number of results returned.
        /// </summary>
        /// <param name="count">The maximum number of results.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> Take(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            _take = count;
            return this;
        }

        /// <summary>
        /// Skips the specified number of results.
        /// </summary>
        /// <param name="count">The number of results to skip.</param>
        /// <returns>The query builder for chaining.</returns>
        public TursoQueryBuilder<TEntity> Skip(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            _skip = count;
            return this;
        }

        #endregion

        #region Execution Methods

        /// <summary>
        /// Executes the query and returns all matching entities.
        /// </summary>
        /// <returns>A list of matching entities.</returns>
        public async Task<List<TEntity>> ToListAsync()
        {
            await _database.EnsureConnectedAsync();
            var (sql, parameters) = BuildQuery();
            return await _database.QueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Executes the query and returns the first matching entity, or null.
        /// </summary>
        /// <returns>The first matching entity or null.</returns>
        public async Task<TEntity?> FirstOrDefaultAsync()
        {
            _take = 1;
            var results = await ToListAsync();
            return results.Count > 0 ? results[0] : null;
        }

        /// <summary>
        /// Executes the query and returns the count of matching entities.
        /// </summary>
        /// <returns>The count of matching entities.</returns>
        public async Task<int> CountAsync()
        {
            await _database.EnsureConnectedAsync();
            var (sql, parameters) = BuildCountQuery();
            var result = await _database.QuerySingleAsync<CountResult>(sql, parameters);
            return result?.Count ?? 0;
        }

        /// <summary>
        /// Checks if any entities match the query.
        /// </summary>
        /// <returns>True if any entities match.</returns>
        public async Task<bool> AnyAsync()
        {
            return await CountAsync() > 0;
        }

        #endregion

        #region Private Methods

        private (string sql, object?[] parameters) BuildQuery()
        {
            var sb = new StringBuilder();
            sb.Append($"SELECT * FROM {_metadata.TableName}");

            var allParameters = new List<object?>();

            // WHERE clauses
            if (_whereClauses.Count > 0)
            {
                var whereClauseParts = new List<string>();
                foreach (var (clause, parameters) in _whereClauses)
                {
                    whereClauseParts.Add($"({clause})");
                    allParameters.AddRange(parameters);
                }
                sb.Append($" WHERE {string.Join(" AND ", whereClauseParts)}");
            }

            // ORDER BY clauses
            if (_orderByClauses.Count > 0)
            {
                sb.Append($" ORDER BY {string.Join(", ", _orderByClauses)}");
            }

            // LIMIT and OFFSET
            if (_take.HasValue)
            {
                sb.Append($" LIMIT {_take.Value}");
            }

            if (_skip.HasValue)
            {
                sb.Append($" OFFSET {_skip.Value}");
            }

            return (sb.ToString(), allParameters.ToArray());
        }

        private (string sql, object?[] parameters) BuildCountQuery()
        {
            var sb = new StringBuilder();
            sb.Append($"SELECT COUNT(*) as Count FROM {_metadata.TableName}");

            var allParameters = new List<object?>();

            // WHERE clauses
            if (_whereClauses.Count > 0)
            {
                var whereClauseParts = new List<string>();
                foreach (var (clause, parameters) in _whereClauses)
                {
                    whereClauseParts.Add($"({clause})");
                    allParameters.AddRange(parameters);
                }
                sb.Append($" WHERE {string.Join(" AND ", whereClauseParts)}");
            }

            return (sb.ToString(), allParameters.ToArray());
        }

        #endregion

        #region Private Types

        private class CountResult
        {
            public int Count { get; set; }
        }

        #endregion

    }

}
