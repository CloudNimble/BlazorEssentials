using CloudNimble.BlazorEssentials.TursoDb;
using System;

namespace CloudNimble.BlazorEssentials.Tests.TursoDb.Sample
{

    /// <summary>
    /// Sample user entity for testing.
    /// </summary>
    [Table("users")]
    public class User
    {

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [PrimaryKey(AutoIncrement = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Column("name")]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Column("email")]
        [Index(Unique = true)]
        public string Email { get; set; } = "";

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        [Column("age")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets whether the user is active.
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets when the user was created.
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

}
