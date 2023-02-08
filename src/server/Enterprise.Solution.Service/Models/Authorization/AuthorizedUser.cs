namespace Enterprise.Solution.Service.Models.Authorization
{
    /// <summary>
    /// DTO for authentication requests
    /// </summary>
    public class AuthorizedUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AuthorizedUser(
            int userId,
            string userName,
            string firstName,
            string lastName
            )
        {
            UserId = userId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
