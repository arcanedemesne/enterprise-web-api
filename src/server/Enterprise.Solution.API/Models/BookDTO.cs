using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of a Book
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BookDTO : BaseDTO
    {
        /// <summary>
        /// Title of the Book
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Publish Date of the Book
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// Base Price of the Book
        /// </summary>
        public decimal BasePrice { get; set; }
        /// <summary>
        /// Id of the Author of the Book
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// Author of the Book
        /// </summary>
        public AuthorDTO Author { get; set; } = null!;
        /// <summary>
        /// CoverId of the Book
        /// </summary>
        public int CoverId { get; set; }
        /// <summary>
        /// The Cover of the Book
        /// </summary>
        public CoverDTO Cover { get; set; } = null!;
    }
}
