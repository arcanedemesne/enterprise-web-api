using AutoMapper;

using Enterprise.Solution.API.Helpers.QueryParams;
using Enterprise.Solution.Service.Services;

using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Solution.API.Controllers.Common
{
    /// <summary>
    /// BaseController implements dependency injection and shared properties for the other controllers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        private readonly IConfiguration? _configuration;
        private readonly ILogger<T>? _logger;
        private readonly IMapper? _mapper;
        private readonly IAuthorService? _authorService;
        private readonly IBookService? _bookService;
        private readonly IArtistService? _artistService;

        /// <summary>
        /// IConfiguration for reaching configuration values inside controllers
        /// </summary>
        protected IConfiguration Configuration => _configuration ?? HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        /// <summary>
        /// ILogger<typeparamref name="T"/> for logging inside controllers
        /// </summary>
        protected ILogger<T> Logger => _logger ?? HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
        /// <summary>
        /// IMapper for auto mapping between dtos and models inside controllers
        /// </summary>
        protected IMapper Mapper => _mapper ?? HttpContext.RequestServices.GetRequiredService<IMapper>();
        /// <summary>
        /// IAuthorService for reaching CRUD operations for authors
        /// </summary>
        protected IAuthorService AuthorService => _authorService ?? HttpContext.RequestServices.GetRequiredService<IAuthorService>();
        /// <summary>
        /// IItemService for reaching CRUD operations for books
        /// </summary>
        protected IBookService BookService => _bookService ?? HttpContext.RequestServices.GetRequiredService<IBookService>();
        /// <summary>
        /// IArtistService for reaching CRUD operations for artists
        /// </summary>
        protected IArtistService ArtistService => _artistService ?? HttpContext.RequestServices.GetRequiredService<IArtistService>();


        private readonly int DEFAULT_PAGE_SIZE = 10;
        private readonly int MAX_PAGE_SIZE = 20;

        /// <summary>
        /// Make sure pageNumber is reset if not provided or invalid
        /// </summary>
        /// <param name="pageNumber"></param>
        protected int SanitizePageNumber(int? pageNumber)
        {
            if (!pageNumber.HasValue || pageNumber.Value <= 0)
            {
                return 1;
            }

            return pageNumber.Value;
        }

        /// <summary>
        /// Make sure pageSize is reset if not provided or invalid
        /// </summary>
        /// <param name="pageSize"></param>
        protected int SanitizePageSize(int? pageSize)
        {
            if (!pageSize.HasValue || pageSize.Value <= 0 || pageSize.Value > MAX_PAGE_SIZE)
            {
                return DEFAULT_PAGE_SIZE;
            }

            return pageSize.Value;
        }
    }
}
