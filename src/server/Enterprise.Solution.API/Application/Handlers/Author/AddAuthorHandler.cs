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
    public class AddAuthorHandler : BaseHandler<AddAuthorHandler>, IRequestHandler<AddAuthorCommand, AuthorDTO>
    {
        private readonly IAuthorService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public AddAuthorHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddAuthorHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(AddAuthorHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<AuthorDTO> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(AddAuthorHandler)} handler invoked with request type {nameof(AddAuthorCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Add);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var emailSubscription = _mapper.Map<Author>(request.AuthorDTO);

            _logger.LogInformation($"Try to add a {nameof(Author)}.");
            var createdAuthor = await _service.AddAsync(emailSubscription);

            if (createdAuthor != null)
            {
                _logger.LogInformation($"{nameof(Author)} with id {createdAuthor.Id} was successfully added.");
                return _mapper.Map<AuthorDTO>(createdAuthor);
            }
            else
            {
                var ex = new NotCreatedException(typeof(Author));
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
