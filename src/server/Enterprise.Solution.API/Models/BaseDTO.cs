using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// The BaseDTO
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BaseDTO
    {
        /// <summary>
        /// Id of the dto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The UTC time this record was created.
        /// </summary>
        public DateTime CreatedTs { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     The identifier of the user that created this record.
        /// </summary>
        public Guid? CreatedBy { get; set; } = null;

        /// <summary>
        ///     The identifier of the user that last modified this record.
        /// </summary>
        public DateTime? ModifiedTs { get; set; } = null;

        /// <summary>
        ///     The UTC time this record was last modified.
        /// </summary>
        public Guid? ModifiedBy { get; set; } = null;

        /// <summary>
        ///     A flag denoting a Soft Delete
        /// </summary>
        public bool? IsDeleted { get; set; } = false;
    }
}