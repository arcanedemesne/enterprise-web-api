using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Models
{
    /// <summary>
    /// a DTO representing the shape of an Notification
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class NotificationDTO_Response : BaseDTO
    {
        /// <summary>
        /// Message of the Notification
        /// </summary>
        public string? Message { get; set; }
        
        /// <summary>
        /// Keycloak Unique Identifier of the User this is assigned to
        /// </summary>
        public Guid AssignedTo { get; set; }
    }
}
