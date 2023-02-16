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
            CreateMap<Author, Models.AuthorDTO>();
            CreateMap<Models.AuthorDTO, Author>();
        }
    }

}
