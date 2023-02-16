using AutoMapper;
using Enterprise.Solution.Data.Models;

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
            CreateMap<Book, Models.BookDTO>();
            CreateMap<Models.BookDTO, Book>();
        }
    }

}
