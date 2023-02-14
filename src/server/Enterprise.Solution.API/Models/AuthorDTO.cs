using Enterprise.Solution.Data.Entities;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Author
    /// </summary>
    public class AuthorDTO : BaseDTO
    {
        /// <summary>
        /// First Name of the Author
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Last Name of the Author
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Calculated Full Name of the Author
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
        /// <summary>
        /// Books written by the Author
        /// </summary>
        public ICollection<BookDTO>? Books { get; set; }
    }
}
