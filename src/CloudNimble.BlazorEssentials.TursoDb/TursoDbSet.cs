using CloudNimble.BlazorEssentials.TursoDb.Query;
using CloudNimble.BlazorEssentials.TursoDb.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents a collection of entities of a specific type in the database.
    /// Provides CRUD operations and querying capabilities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type this set manages.</typeparam>
    public class TursoDbSet<TEntity> : ITursoDbSet where TEntity : class, new()
    {

        #region Private Members

        private readonly TursoDatabase _database;
        private readonly EntityMetadata _metadata;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent database.
        /// </summary>
        public TursoDatabase Database => _database;

        /// <summary>
        /// Gets the table name for this entity type.
        /// </summary>
        public string TableName => _metadata.TableName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TursoDbSet{TEntity}"/> class.
        /// </summary>
        /// <param name="database">The parent database.</param>
        /// <exception cref="ArgumentNullException">Thrown when database is null.</exception>
        public TursoDbSet(TursoDatabase database)
        {
            ArgumentNullException.ThrowIfNull(database);

            _database = database;
            _metadata = EntityMetadataCache.GetOrCreate<TEntity>();

            // Register this DbSet with the database
            database.DbSets.Add(this);
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Creates a new query builder for this entity set.
        /// </summary>
        /// <returns>A query builder for fluent query construction.</returns>
        public TursoQueryBuilder<TEntity> Query()
        {
            return new TursoQueryBuilder<TEntity>(_database);
        }

        /// <summary>
        /// Creates a query builder with an initial WHERE clause.
        /// </summary>
        /// <param name="clause">The WHERE clause (e.g., "name = ?" or "age > ?").</param>
        /// <param name="parameters">The parameters for the clause.</param>
        /// <returns>A query builder for fluent query construction.</returns>
        public TursoQueryBuilder<TEntity> Where(string clause, params object?[] parameters)
        {
            return Query().Where(clause, parameters);
        }

        /// <summary>
        /// Creates a query builder with an initial ORDER BY clause (ascending).
        /// </summary>
        /// <param name="column">The column to order by.</param>
        /// <returns>A query builder for fluent query construction.</returns>
        public TursoQueryBuilder<TEntity> OrderBy(string column)
        {
            return Query().OrderBy(column);
        }

        /// <summary>
        /// Creates a query builder with an initial ORDER BY clause (descending).
        /// </summary>
        /// <param name="column">The column to order by.</param>
        /// <returns>A query builder for fluent query construction.</returns>
        public TursoQueryBuilder<TEntity> OrderByDescending(string column)
        {
            return Query().OrderByDescending(column);
        }

        /// <summary>
        /// Retrieves all entities from the table.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        public async Task<List<TEntity>> ToListAsync()
        {
            await _database.EnsureConnectedAsync();
            var sql = SqlGenerator.GenerateSelectAll(_metadata);
            return await _database.QueryAsync<TEntity>(sql);
        }

        /// <summary>
        /// Gets the count of entities in the table.
        /// </summary>
        /// <returns>The count of entities.</returns>
        public async Task<int> CountAsync()
        {
            await _database.EnsureConnectedAsync();
            var sql = SqlGenerator.GenerateCount(_metadata);
            var result = await _database.QuerySingleAsync<CountResult>(sql);
            return result?.Count ?? 0;
        }

        /// <summary>
        /// Finds an entity by its primary key.
        /// </summary>
        /// <param name="key">The primary key value.</param>
        /// <returns>The entity if found, otherwise null.</returns>
        public async Task<TEntity?> FindAsync(object key)
        {
            ArgumentNullException.ThrowIfNull(key);
            await _database.EnsureConnectedAsync();

            var sql = SqlGenerator.GenerateSelectByKey(_metadata);
            return await _database.QuerySingleAsync<TEntity>(sql, key);
        }

        /// <summary>
        /// Executes a raw SQL query against this entity's table.
        /// </summary>
        /// <param name="sql">The SQL query.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>A list of matching entities.</returns>
        public async Task<List<TEntity>> FromSqlAsync(string sql, params object?[] parameters)
        {
            await _database.EnsureConnectedAsync();
            return await _database.QueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Gets the first entity matching the optional filter, or null if none.
        /// </summary>
        /// <returns>The first entity or null.</returns>
        public async Task<TEntity?> FirstOrDefaultAsync()
        {
            await _database.EnsureConnectedAsync();
            var sql = $"{SqlGenerator.GenerateSelectAll(_metadata)} LIMIT 1";
            return await _database.QuerySingleAsync<TEntity>(sql);
        }

        #endregion

        #region CRUD Methods

        /// <summary>
        /// Adds a new entity to the table.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The execution result containing the last insert ID.</returns>
        public async Task<TursoResult> AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _database.EnsureConnectedAsync();

            var (columns, placeholders, parameters) = _metadata.GetInsertData(entity);
            var sql = SqlGenerator.GenerateInsert(_metadata, columns, placeholders);
            var result = await _database.ExecuteAsync(sql, parameters);

            // Update the entity's primary key if auto-increment
            if (_metadata.PrimaryKey?.AutoIncrement == true && result.LastInsertRowId > 0)
            {
                _metadata.SetPrimaryKeyValue(entity, result.LastInsertRowId);
            }

            return result;
        }

        /// <summary>
        /// Adds multiple entities to the table in a batch.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>The number of entities added.</returns>
        public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            await _database.EnsureConnectedAsync();

            var count = 0;
            foreach (var entity in entities)
            {
                await AddAsync(entity);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Updates an existing entity in the table.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The execution result.</returns>
        public async Task<TursoResult> UpdateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _database.EnsureConnectedAsync();

            var (setClause, parameters) = _metadata.GetUpdateData(entity);
            var sql = SqlGenerator.GenerateUpdate(_metadata, setClause);
            return await _database.ExecuteAsync(sql, parameters);
        }

        /// <summary>
        /// Removes an entity from the table.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>The execution result.</returns>
        public async Task<TursoResult> RemoveAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _database.EnsureConnectedAsync();

            var key = _metadata.GetPrimaryKeyValue(entity);
            var sql = SqlGenerator.GenerateDelete(_metadata);
            return await _database.ExecuteAsync(sql, key);
        }

        /// <summary>
        /// Removes an entity by its primary key.
        /// </summary>
        /// <param name="key">The primary key value.</param>
        /// <returns>The execution result.</returns>
        public async Task<TursoResult> RemoveByKeyAsync(object key)
        {
            ArgumentNullException.ThrowIfNull(key);
            await _database.EnsureConnectedAsync();

            var sql = SqlGenerator.GenerateDelete(_metadata);
            return await _database.ExecuteAsync(sql, key);
        }

        /// <summary>
        /// Removes multiple entities from the table.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        /// <returns>The number of entities removed.</returns>
        public async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            await _database.EnsureConnectedAsync();

            var count = 0;
            foreach (var entity in entities)
            {
                var result = await RemoveAsync(entity);
                count += result.RowsAffected;
            }
            return count;
        }

        #endregion

        #region ITursoDbSet Implementation

        /// <inheritdoc/>
        public EntityMetadata GetEntityMetadata() => _metadata;

        #endregion

        #region Private Types

        private class CountResult
        {
            public int Count { get; set; }
        }

        #endregion

    }

}
