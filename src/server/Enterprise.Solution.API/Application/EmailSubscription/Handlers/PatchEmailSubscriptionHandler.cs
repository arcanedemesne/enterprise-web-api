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
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(PatchEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<PatchEmailSubscriptionCommand>();

            LogCheckIfExists<EmailSubscription>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<EmailSubscription>(request.Id);

            LogTryServiceRequest<EmailSubscription>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);
            if (entity == null) LogAndThrowNotFoundException<EmailSubscription>(request.Id);

            var patchedEntity = _mapper.Map<EmailSubscriptionDTO>(entity);

            LogTryServiceRequest<EmailSubscription>(RequestType.Patch, request.Id);
            request.JsonPatchDocument.ApplyTo(patchedEntity, request.ModelState);

            if (!request.ModelState.IsValid || !request.TryValidateModel(patchedEntity))
                LogAndThrowInvalidModelException<EmailSubscription>(RequestType.Patch);

            _mapper.Map(patchedEntity, entity);

            try
            {
                LogTryServiceRequest<EmailSubscription>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
            }
            catch (Exception)
            {
                LogAndThrowNotPatchedException<EmailSubscription>(request.Id);
            }
            return;
        }
    }
}
