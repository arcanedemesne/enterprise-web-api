using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Patch Command
    /// </summary>
    public class PatchArtistHandler : BaseHandler<PatchArtistHandler>, IRequestHandler<PatchArtistCommand>
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
        public PatchArtistHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<PatchArtistHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            LogInsideConstructor();
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchArtistCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<PatchArtistCommand>();

            LogCheckIfExists<Artist>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Artist>(request.Id);

            LogTryServiceRequest<Artist>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null) LogAndThrowNotFoundException<Artist>(request.Id);

            var patchedEntity = _mapper.Map<ArtistDTO>(entity);

            LogTryServiceRequest<Artist>(RequestType.Patch, request.Id);
            request.JsonPatchDocument.ApplyTo(patchedEntity, request.ModelState);

            if (!request.ModelState.IsValid || !request.TryValidateModel(patchedEntity))
                LogAndThrowInvalidModelException<Artist>(RequestType.Patch);

            _mapper.Map(patchedEntity, entity);

            try
            {
                LogTryServiceRequest<Artist>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
            }
            catch (Exception)
            {
                LogAndThrowNotPatchedException<Artist>(request.Id);
            }
            return;
        }
    }
}
