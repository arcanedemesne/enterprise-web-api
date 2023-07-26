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
            CreateMap<Book, Models.BookDTO_Request>();
            CreateMap<Models.BookDTO_Request, Book>();

            CreateMap<Book, Models.BookDTO_Response>();
            CreateMap<Models.BookDTO_Response, Book>();
        }
    }

}
