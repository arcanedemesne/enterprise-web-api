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
    public class AddNotificationHandler : BaseHandler<AddNotificationHandler>, IRequestHandler<AddNotificationCommand, NotificationDTO_Request>
    {
        private readonly INotificationService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public AddNotificationHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddNotificationHandler> logger,
            IMapper mapper,
            INotificationService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<NotificationDTO_Request> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddNotificationCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Notification>(RequestType.Add);

            var entity = _mapper.Map<Notification>(request.NotificationDTO);

            LogTryServiceRequest<Notification>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<Notification>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<NotificationDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<Notification>();
            return new NotificationDTO_Request();
        }
    }
}
