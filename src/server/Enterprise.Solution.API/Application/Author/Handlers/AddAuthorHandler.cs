using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Add Command
    /// </summary>
    public class AddAuthorHandler : BaseHandler<AddAuthorHandler>, IRequestHandler<AddAuthorCommand, AuthorDTO_Request>
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
            LogInsideConstructor();
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<AuthorDTO_Request> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddAuthorCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Author>(RequestType.Add);

            var entity = _mapper.Map<Author>(request.AuthorDTO);

            LogTryServiceRequest<Author>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<Author>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<AuthorDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<Author>();
            return new AuthorDTO_Request();
        }
    }
}
