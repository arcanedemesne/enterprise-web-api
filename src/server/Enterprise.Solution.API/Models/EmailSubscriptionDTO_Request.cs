using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an EmailSubscription
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EmailSubscriptionDTO_Request : BaseDTO
    {
        /// <summary>
        /// First Name of the EmailSubscription
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Last Name of the EmailSubscription
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Calculated Full Name of the EmailSubscription
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
        /// <summary>
        /// Email Address of the EmailSubscription
        /// </summary>
        public string? EmailAddress { get; set; }
    }
}
