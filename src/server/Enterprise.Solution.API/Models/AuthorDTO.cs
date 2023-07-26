using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Author
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
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
        /// Full Name of the Author
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Books written by the Author
        /// </summary>
        public ICollection<BookDTO_Response>? Books { get; set; }
    }
}
