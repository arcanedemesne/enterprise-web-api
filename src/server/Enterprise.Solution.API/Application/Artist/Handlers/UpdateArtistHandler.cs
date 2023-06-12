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
    /// Handler for Update Command
    /// </summary>
    public class UpdateArtistHandler : BaseHandler<UpdateArtistHandler>, IRequestHandler<UpdateArtistCommand>
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
        public UpdateArtistHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateArtistHandler> logger,
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
        public async Task Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateArtistCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Artist>(RequestType.Update);

            LogCheckIfExists<Artist>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Artist>(request.Id);

            if (!request.Id.Equals(request.ArtistDTO.Id))
                LogAndThrowNotUpdatedException<Artist>(request.Id);

            LogTryServiceRequest<Artist>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<Artist>(request.Id);
            _mapper.Map(request.ArtistDTO, entity);

            try
            {
                LogTryServiceRequest<Artist>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<Artist>(request.Id);
            }
        }
    }
}
