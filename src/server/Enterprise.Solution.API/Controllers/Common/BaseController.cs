using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Enterprise.Solution.Email.Service;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

namespace Enterprise.Solution.API.Controllers.Common
{
    /// <summary>
    /// BaseController implements dependency injection and shared properties for the other controllers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        /// <summary>
        /// SolutionSettings from the appsettings.json file
        /// </summary>
        protected SolutionSettings _solutionSettings;

        /// <summary>
        /// IMediator for mediting api requests, validation, and service actions
        /// </summary>
        protected readonly IMediator _mediator;

        /// <summary>
        /// ILogger for logging
        /// </summary>
        private readonly ILogger<T>? _logger;

        /// <summary>
        /// IMapper for mapping entities and dtos
        /// </summary>
        private readonly IMapper? _mapper;

        /// <summary>
        /// Constructor for the BaseController with MediatR
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public BaseController(IOptions<SolutionSettings> solutionSettings, IMediator mediator)
        {
            _solutionSettings = solutionSettings.Value;
            _mediator = mediator;
        }

        private readonly IEmailService? _emailService;
        private readonly IAuthorService? _authorService;
        private readonly IBookService? _bookService;
        private readonly IArtistService? _artistService;

        /// <summary>
        /// ILogger<typeparamref name="T"/> for logging inside controllers
        /// </summary>
        protected ILogger<T> Logger => _logger ?? HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
        /// <summary>
        /// IMapper for auto mapping between dtos and models inside controllers
        /// </summary>
        protected IMapper Mapper => _mapper ?? HttpContext.RequestServices.GetRequiredService<IMapper>();
        /// <summary>
        /// IEmailService for emailing 
        /// </summary>
        protected IEmailService EmailService => _emailService ?? HttpContext.RequestServices.GetRequiredService<IEmailService>();
        /// <summary>
        /// IAuthorService for reaching CRUD operations for authors
        /// </summary>
        protected IAuthorService AuthorService => _authorService ?? HttpContext.RequestServices.GetRequiredService<IAuthorService>();
        /// <summary>
        /// IBookService for reaching CRUD operations for books
        /// </summary>
        protected IBookService BookService => _bookService ?? HttpContext.RequestServices.GetRequiredService<IBookService>();
        /// <summary>
        /// IArtistService for reaching CRUD operations for artists
        /// </summary>
        protected IArtistService ArtistService => _artistService ?? HttpContext.RequestServices.GetRequiredService<IArtistService>();
    }
}
