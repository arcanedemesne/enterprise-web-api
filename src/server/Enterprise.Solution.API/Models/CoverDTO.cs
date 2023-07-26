using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// A DTO representing the shape of a Cover
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class CoverDTO : BaseDTO
    {
        /// <summary>
        /// DesignIdeas of the Cover
        /// </summary>
        public string? DesignIdeas { get; set; }
        /// <summary>
        /// Is Cover Digital Only?
        /// </summary>
        public bool DigitalOnly { get; set; }
        /// <summary>
        /// Related BookId of the Cover
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Related Book of the Cover
        /// </summary>
        public BookDTO_Response Book { get; set; } = null!;
        /// <summary>
        /// Artists of the Cover
        /// </summary>
        public ICollection<ArtistDTO>? Artists { get; set; }
    }
}
