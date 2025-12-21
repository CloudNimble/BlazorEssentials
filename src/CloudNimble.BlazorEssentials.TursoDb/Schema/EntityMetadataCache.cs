using System;
using System.Collections.Concurrent;

namespace CloudNimble.BlazorEssentials.TursoDb.Schema
{

    /// <summary>
    /// Caches entity metadata for performance.
    /// </summary>
    public static class EntityMetadataCache
    {

        private static readonly ConcurrentDictionary<Type, EntityMetadata> _cache = new();

        /// <summary>
        /// Gets or creates metadata for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The entity metadata.</returns>
        public static EntityMetadata GetOrCreate<TEntity>() where TEntity : class
        {
            return GetOrCreate(typeof(TEntity));
        }

        /// <summary>
        /// Gets or creates metadata for the specified entity type.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <returns>The entity metadata.</returns>
        public static EntityMetadata GetOrCreate(Type entityType)
        {
            return _cache.GetOrAdd(entityType, t => new EntityMetadata(t));
        }

        /// <summary>
        /// Clears all cached metadata.
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }

    }

}
