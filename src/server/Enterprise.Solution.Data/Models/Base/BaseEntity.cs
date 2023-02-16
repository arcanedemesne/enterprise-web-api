using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.Models.Base
{
    [PrimaryKey("Id")]
    public abstract class BaseEntity
    {
        /// <summary>
        ///     The Id of this record
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        ///     The UTC time this record was created.
        /// </summary>
        public DateTime CreatedTs { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     The identifier of the user that created this record.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        ///     The identifier of the user that last modified this record.
        /// </summary>
        public DateTime ModifiedTs { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     The UTC time this record was last modified.
        /// </summary>
        public Guid ModifiedBy { get; set; }
    }
}