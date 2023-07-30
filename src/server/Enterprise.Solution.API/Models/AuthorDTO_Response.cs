using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Author
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AuthorDTO_Response : BaseDTO
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
        /// Calculated Full Name
        /// </summary>
        public string? FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Books written by the Author
        /// </summary>
        public ICollection<BookDTO_Response>? Books { get; set; }
    }
}
