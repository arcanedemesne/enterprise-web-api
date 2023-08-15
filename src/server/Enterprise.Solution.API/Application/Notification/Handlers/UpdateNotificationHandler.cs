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
    public class UpdateNotificationHandler : BaseHandler<UpdateNotificationHandler>, IRequestHandler<UpdateNotificationCommand>
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
        public UpdateNotificationHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateNotificationHandler> logger,
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
        public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateNotificationCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<Notification>(RequestType.Update);

            LogCheckIfExists<Notification>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Notification>(request.Id);

            if (!request.Id.Equals(request.NotificationDTO.Id))
                LogAndThrowNotUpdatedException<Notification>(request.Id);

            LogTryServiceRequest<Notification>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<Notification>(request.Id);
            _mapper.Map(request.NotificationDTO, entity);

            try
            {
                LogTryServiceRequest<Notification>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<Notification>(request.Id);
            }
        }
    }
}
