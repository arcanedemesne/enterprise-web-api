using AutoMapper;
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// NotificationProfile for automapper
    /// </summary>
    public class NotificationProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public NotificationProfile()
        {
            CreateMap<Notification, Models.NotificationDTO_Request>();
            CreateMap<Models.NotificationDTO_Request, Notification>();

            CreateMap<Notification, Models.NotificationDTO_Response>();
            CreateMap<Models.NotificationDTO_Response, Notification>();
        }
    }

}
