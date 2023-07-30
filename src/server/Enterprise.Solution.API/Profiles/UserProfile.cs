using AutoMapper;
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// UserProfile for automapper
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public UserProfile()
        {
            CreateMap<User, Models.UserDTO_Request>();
            CreateMap<Models.UserDTO_Request, User>();

            CreateMap<User, Models.UserDTO_Response>();
            CreateMap<Models.UserDTO_Response, User>();
        }
    }

}
