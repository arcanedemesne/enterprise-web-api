using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Get By Id Query
    /// </summary>
    public class GetNotificationByIdHandler : BaseHandler<GetNotificationByIdHandler>, IRequestHandler<GetNotificationByIdQuery, NotificationDTO_Response>
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
        public GetNotificationByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetNotificationByIdHandler> logger,
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
        /// <returns></returns>
        public async Task<NotificationDTO_Response> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<GetNotificationByIdQuery>();

            LogTryServiceRequest<Notification>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotFoundException<Notification>(request.Id);

            return _mapper.Map<NotificationDTO_Response>(entity);
        }
    }
}
