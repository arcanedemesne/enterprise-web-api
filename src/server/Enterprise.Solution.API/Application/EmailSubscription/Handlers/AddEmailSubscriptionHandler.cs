using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Add Command
    /// </summary>
    public class AddEmailSubscriptionHandler : BaseHandler<AddEmailSubscriptionHandler>, IRequestHandler<AddEmailSubscriptionCommand, EmailSubscriptionDTO_Request>
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
        public AddEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddEmailSubscriptionHandler> logger,
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
        public async Task<EmailSubscriptionDTO_Request> Handle(AddEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddEmailSubscriptionCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<EmailSubscription>(RequestType.Add);

            var entity = _mapper.Map<EmailSubscription>(request.EmailSubscriptionDTO);

            LogTryServiceRequest<EmailSubscription>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<EmailSubscription>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<EmailSubscriptionDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<EmailSubscription>();
            return new EmailSubscriptionDTO_Request();
        }
    }
}
