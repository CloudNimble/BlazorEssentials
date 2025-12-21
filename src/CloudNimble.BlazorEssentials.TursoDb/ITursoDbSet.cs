using CloudNimble.BlazorEssentials.TursoDb.Schema;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Interface for TursoDbSet, used for database discovery.
    /// </summary>
    public interface ITursoDbSet
    {

        /// <summary>
        /// Gets the entity metadata for this DbSet.
        /// </summary>
        /// <returns>The entity metadata.</returns>
        EntityMetadata GetEntityMetadata();

    }

}
