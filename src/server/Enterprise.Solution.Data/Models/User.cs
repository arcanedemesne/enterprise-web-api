using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class User : BaseEntity
    {
        public Guid KeycloakUniqueIdentifier { get; set; }

        /// <summary>
        ///     The UserName of the user (usually used to sign in)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     The First Name of the User
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     The Last Name of the User
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     The Email address of the User
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        ///     Roles the User has or belongs to
        /// </summary>
        public ICollection<UserRole> Roles {  get; set; }
    }
}
