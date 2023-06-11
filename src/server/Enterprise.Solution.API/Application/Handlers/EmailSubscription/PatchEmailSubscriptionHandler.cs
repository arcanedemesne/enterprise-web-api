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
    public class PatchEmailSubscriptionHandler : BaseHandler<PatchEmailSubscriptionHandler>, IRequestHandler<PatchEmailSubscriptionCommand>
    {
        private readonly IEmailSubscriptionService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public PatchEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<PatchEmailSubscriptionHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(PatchEmailSubscriptionHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(PatchEmailSubscriptionHandler)} handler invoked with request type {nameof(PatchEmailSubscriptionCommand)}.");

            _logger.LogInformation($"Check if {nameof(EmailSubscription)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null)
            {
                var ex = new NotFoundException(nameof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var patchedEntity = _mapper.Map<EmailSubscriptionDTO>(entity);

            _logger.LogInformation($"Try to apply patch {nameof(EmailSubscription)} with id {request.Id} to model state.");
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
                _logger.LogInformation($"Try to update {nameof(EmailSubscription)} with id {request.Id} using ops & values from patch command.");
                await _service.UpdateAsync(entity);

            }
            catch (Exception ex)
            {
                var nEx = new NotPatchedException(typeof(EmailSubscription), request.Id);
                var fEx = new NotPatchedException(nEx.Message, ex);
                _logger.LogError(fEx.Message, fEx.InnerException);
                throw fEx;
            }
            return;
        }
    }
}
