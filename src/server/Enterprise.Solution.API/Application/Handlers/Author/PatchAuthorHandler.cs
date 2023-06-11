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
    public class PatchAuthorHandler : BaseHandler<PatchAuthorHandler>, IRequestHandler<PatchAuthorCommand>
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
        public PatchAuthorHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<PatchAuthorHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(PatchAuthorHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(PatchAuthorHandler)} handler invoked with request type {nameof(PatchAuthorCommand)}.");

            _logger.LogInformation($"Check if {nameof(Author)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var ex = new NotFoundException(nameof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var patchedEntity = _mapper.Map<AuthorDTO>(entity);

            _logger.LogInformation($"Try to apply patch {nameof(Author)} with id {request.Id} to model state.");
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
                _logger.LogInformation($"Try to update {nameof(Author)} with id {request.Id} using ops & values from patch command.");
                await _service.UpdateAsync(entity);

            }
            catch (Exception ex)
            {
                var nEx = new NotPatchedException(typeof(Author), request.Id);
                var fEx = new NotPatchedException(nEx.Message, ex);
                _logger.LogError(fEx.Message, fEx.InnerException);
                throw fEx;
            }
            return;
        }
    }
}
