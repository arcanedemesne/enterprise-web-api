namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of a Book
    /// </summary>
    public class BookDTO
    {
        /// <summary>
        /// Id of the Book
        /// </summary>
        public int Id { get; set; }
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
    }
}
