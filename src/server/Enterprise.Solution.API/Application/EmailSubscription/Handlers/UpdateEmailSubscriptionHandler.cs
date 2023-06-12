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
    public class UpdateEmailSubscriptionHandler : BaseHandler<UpdateEmailSubscriptionHandler>, IRequestHandler<UpdateEmailSubscriptionCommand>
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
        public UpdateEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateEmailSubscriptionHandler> logger,
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
        public async Task Handle(UpdateEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateEmailSubscriptionCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<EmailSubscription>(RequestType.Update);

            LogCheckIfExists<EmailSubscription>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<EmailSubscription>(request.Id);

            if (!request.Id.Equals(request.EmailSubscriptionDTO.Id))
                LogAndThrowNotUpdatedException<EmailSubscription>(request.Id);

            LogTryServiceRequest<EmailSubscription>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<EmailSubscription>(request.Id);
            _mapper.Map(request.EmailSubscriptionDTO, entity);

            try
            {
                LogTryServiceRequest<EmailSubscription>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<EmailSubscription>(request.Id);
            }
        }
    }
}
