using AutoMapper;
using Enterprise.Solution.Data.Models;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// AuthorProfile for automapper
    /// </summary>
    public class AuthorProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public AuthorProfile()
        {
            CreateMap<Author, Models.AuthorDTO_Request>();
            CreateMap<Models.AuthorDTO_Request, Author>();

            CreateMap<Author, Models.AuthorDTO_Response>();
            CreateMap<Models.AuthorDTO_Response, Author>();
        }
    }

}
