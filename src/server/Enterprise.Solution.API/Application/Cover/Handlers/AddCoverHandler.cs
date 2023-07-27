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
    public class AddCoverHandler : BaseHandler<AddCoverHandler>, IRequestHandler<AddCoverCommand, CoverDTO_Request>
    {
        private readonly ICoverService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public AddCoverHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddCoverHandler> logger,
            IMapper mapper,
            ICoverService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<CoverDTO_Request> Handle(AddCoverCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddCoverCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Cover>(RequestType.Add);

            var entity = _mapper.Map<Cover>(request.CoverDTO);

            LogTryServiceRequest<Cover>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<Cover>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<CoverDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<Cover>();
            return new CoverDTO_Request();
        }
    }
}
