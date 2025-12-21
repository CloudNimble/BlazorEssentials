using CloudNimble.BlazorEssentials.TursoDb;
using Microsoft.JSInterop;

namespace CloudNimble.BlazorEssentials.Tests.TursoDb.Sample
{

    /// <summary>
    /// Sample database for testing TursoDb.
    /// </summary>
    public class SampleDatabase : TursoDatabase
    {

        /// <summary>
        /// Gets the Users DbSet.
        /// </summary>
        public TursoDbSet<User> Users { get; }

        /// <summary>
        /// Gets the Posts DbSet.
        /// </summary>
        public TursoDbSet<Post> Posts { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDatabase"/> class.
        /// </summary>
        /// <param name="jsRuntime">The JavaScript runtime.</param>
        public SampleDatabase(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            Users = new TursoDbSet<User>(this);
            Posts = new TursoDbSet<Post>(this);
        }

    }

}
