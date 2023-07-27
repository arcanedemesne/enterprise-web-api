using AutoMapper;
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// EmailSubscriptionProfile for automapper
    /// </summary>
    public class EmailSubscriptionProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public EmailSubscriptionProfile()
        {
            CreateMap<EmailSubscription, Models.EmailSubscriptionDTO_Request>();
            CreateMap<Models.EmailSubscriptionDTO_Request, EmailSubscription>();

            CreateMap<EmailSubscription, Models.EmailSubscriptionDTO_Response>();
            CreateMap<Models.EmailSubscriptionDTO_Response, EmailSubscription>();
        }
    }

}
