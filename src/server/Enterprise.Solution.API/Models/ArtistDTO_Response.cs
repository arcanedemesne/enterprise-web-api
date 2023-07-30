using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Artist
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ArtistDTO_Response : BaseDTO
    {
        /// <summary>
        /// FirstName of the Artist
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// LastName of the Artist
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Calculated Full Name
        /// </summary>
        public string? FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Covers made by Artist
        /// </summary>
        public ICollection<CoverDTO_Response>? Covers { get; set; }
    }
}
