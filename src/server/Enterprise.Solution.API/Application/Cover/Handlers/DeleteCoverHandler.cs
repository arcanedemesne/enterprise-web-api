using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Delete Command
    /// </summary>
    public class DeleteCoverHandler : BaseHandler<DeleteCoverHandler>, IRequestHandler<DeleteCoverCommand>
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
        public DeleteCoverHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteCoverHandler> logger,
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
        /// <returns></returns>
        public async Task Handle(DeleteCoverCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteCoverCommand>();

            LogCheckIfExists<Cover>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Cover>(request.Id);

            LogTryServiceRequest<Cover>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<Cover>(request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted) LogServiceRequestSuccess<Cover>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<Cover>(request.Id);
        }
    }
}
