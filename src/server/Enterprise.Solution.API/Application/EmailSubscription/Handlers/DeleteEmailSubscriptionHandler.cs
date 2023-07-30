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
    public class DeleteEmailSubscriptionHandler : BaseHandler<DeleteEmailSubscriptionHandler>, IRequestHandler<DeleteEmailSubscriptionCommand>
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
        public DeleteEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteEmailSubscriptionHandler> logger,
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
        /// <returns></returns>
        public async Task Handle(DeleteEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteEmailSubscriptionCommand>();

            LogCheckIfExists<EmailSubscription>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<EmailSubscription>(request.Id);

            LogTryServiceRequest<EmailSubscription>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<EmailSubscription>(request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted) LogServiceRequestSuccess<EmailSubscription>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<EmailSubscription>(request.Id);
        }
    }
}
