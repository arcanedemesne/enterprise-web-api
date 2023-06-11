using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Add Command
    /// </summary>
    public class AddBookHandler : BaseHandler<AddBookHandler>, IRequestHandler<AddBookCommand, BookDTO>
    {
        private readonly IBookService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public AddBookHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddBookHandler> logger,
            IMapper mapper,
            IBookService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(AddBookHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<BookDTO> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(AddBookHandler)} handler invoked with request type {nameof(AddBookCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Add);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var emailSubscription = _mapper.Map<Book>(request.BookDTO);

            _logger.LogInformation($"Try to add a {nameof(Book)}.");
            var createdBook = await _service.AddAsync(emailSubscription);

            if (createdBook != null)
            {
                _logger.LogInformation($"{nameof(Book)} with id {createdBook.Id} was successfully added.");
                return _mapper.Map<BookDTO>(createdBook);
            }
            else
            {
                var ex = new NotCreatedException(typeof(Book));
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
