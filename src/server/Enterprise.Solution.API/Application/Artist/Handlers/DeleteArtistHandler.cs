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
    public class DeleteArtistHandler : BaseHandler<DeleteArtistHandler>, IRequestHandler<DeleteArtistCommand>
    {
        private readonly IArtistService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public DeleteArtistHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteArtistHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteArtistCommand>();

            LogCheckIfExists<Artist>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Artist>(request.Id);

            LogTryServiceRequest<Artist>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<Artist>(request.Id);
            var stillExists = await _service.ExistsAsync(request.Id);

            if (!stillExists) LogServiceRequestSuccess<Artist>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<Artist>(request.Id);
        }
    }
}
