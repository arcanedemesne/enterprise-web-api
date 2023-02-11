using AutoMapper;

namespace Enterprise.Solution.API.Profiles
{
    /// <summary>
    /// BookProfile for automapper
    /// </summary>
    public class BookProfile : Profile
    {
        /// <summary>
        /// Constructor for creating mapping profiles 
        /// </summary>
        public BookProfile()
        {
            CreateMap<Data.Entities.Book, Models.BookDTO>();
            CreateMap<Models.BookDTO, Data.Entities.Book>();
        }
    }

}
