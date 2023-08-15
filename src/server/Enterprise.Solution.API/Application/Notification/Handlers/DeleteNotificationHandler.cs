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
    public class DeleteNotificationHandler : BaseHandler<DeleteNotificationHandler>, IRequestHandler<DeleteNotificationCommand>
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
        public DeleteNotificationHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteNotificationHandler> logger,
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
        public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteNotificationCommand>();

            LogCheckIfExists<Notification>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Notification>(request.Id);

            LogTryServiceRequest<Notification>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<Notification>(request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted) LogServiceRequestSuccess<Notification>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<Notification>(request.Id);
        }
    }
}
