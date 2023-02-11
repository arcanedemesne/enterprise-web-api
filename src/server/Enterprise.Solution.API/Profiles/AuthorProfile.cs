using AutoMapper;

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
            CreateMap<Data.Entities.Author, Models.AuthorDTO>();
            CreateMap<Models.AuthorDTO, Data.Entities.Author>();
        }
    }

}
