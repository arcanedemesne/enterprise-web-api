using Enterprise.Solution.Data.Entities;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Artist
    /// </summary>
    public class ArtistDTO : BaseDTO
    {
        /// <summary>
        /// FirstName of the Artist
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// LastName of the Artist
        /// </summary>
        public string? LastName { get; set; }

        //public ICollection<Cover> Covers { get; set; }
    }
}
