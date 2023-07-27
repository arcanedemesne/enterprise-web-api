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
        /// Full Name of the Author
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Covers made by Artist
        /// </summary>
        public ICollection<CoverDTO_Response>? Covers { get; set; }
    }
}
