using CloudNimble.BlazorEssentials.TursoDb;
using System;

namespace CloudNimble.BlazorEssentials.Tests.TursoDb.Sample
{

    /// <summary>
    /// Sample post entity for testing.
    /// </summary>
    [Table("posts")]
    public class Post
    {

        /// <summary>
        /// Gets or sets the post ID.
        /// </summary>
        [PrimaryKey(AutoIncrement = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID who created the post.
        /// </summary>
        [Column("user_id")]
        [Index]
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the post title.
        /// </summary>
        [Column("title")]
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the post content.
        /// </summary>
        [Column("content")]
        public string Content { get; set; } = "";

        /// <summary>
        /// Gets or sets when the post was published.
        /// </summary>
        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

    }

}
