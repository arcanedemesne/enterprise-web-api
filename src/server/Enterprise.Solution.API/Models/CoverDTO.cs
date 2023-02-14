namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// A DTO representing the shape of a Cover
    /// </summary>
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
        /// Artists of the Cover
        /// </summary>
        public ICollection<ArtistDTO>? Artists { get; set; }
    }
}
