using Enterprise.Solution.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Artist
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UserDTO_Request : BaseDTO
    {
        /// <summary>
        ///     The Unique Identifier for Keycloak User
        /// </summary>
        public Guid? KeycloakUniqueIdentifier { get; set; }

        /// <summary>
        ///     The UserName of the user (usually used to sign in)
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        ///     The First Name of the User
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        ///     The Last Name of the User
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        ///     The Email address of the User
        /// </summary>
        public string? EmailAddress { get; set; }
    }
}
