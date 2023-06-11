using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

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
            _logger.LogInformation($"{nameof(PatchArtistHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchArtistCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(PatchArtistHandler)} handler invoked with request type {nameof(PatchArtistCommand)}.");

            _logger.LogInformation($"Check if {nameof(Artist)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(Artist), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var ex = new NotFoundException(nameof(Artist), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var patchedEntity = _mapper.Map<ArtistDTO>(entity);

            _logger.LogInformation($"Try to apply patch {nameof(Artist)} with id {request.Id} to model state.");
            request.JsonPatchDocument.ApplyTo(patchedEntity, request.ModelState);

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Patch);
                _logger.LogError(ex.Message);
                throw ex;
            }

            if (!request.TryValidateModel(patchedEntity))
            {
                var ex = new InvalidModelException(RequestType.Patch);
                _logger.LogError(ex.Message);
                throw ex;
            }

            _mapper.Map(patchedEntity, entity);

            try
            {
                _logger.LogInformation($"Try to update {nameof(Artist)} with id {request.Id} using ops & values from patch command.");
                await _service.UpdateAsync(entity);

            }
            catch (Exception ex)
            {
                var nEx = new NotPatchedException(typeof(Artist), request.Id);
                var fEx = new NotPatchedException(nEx.Message, ex);
                _logger.LogError(fEx.Message, fEx.InnerException);
                throw fEx;
            }
            return;
        }
    }
}
