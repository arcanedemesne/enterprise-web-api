using AutoMapper;
using Enterprise.Solution.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Solution.API.Controllers
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
        private readonly IItemService? _itemService;

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
        /// IAuthorService for reaching CRUD operations for items
        /// </summary>
        protected IAuthorService AuthorService => _authorService ?? HttpContext.RequestServices.GetRequiredService<IAuthorService>();
        /// <summary>
        /// IItemService for reaching CRUD operations for items
        /// </summary>
        protected IItemService ItemService => _itemService ?? HttpContext.RequestServices.GetRequiredService<IItemService>();
    }
}
