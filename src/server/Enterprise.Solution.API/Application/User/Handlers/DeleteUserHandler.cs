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
    public class DeleteUserHandler : BaseHandler<DeleteUserHandler>, IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public DeleteUserHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteUserHandler> logger,
            IMapper mapper,
            IUserService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteUserCommand>();

            LogCheckIfExists<User>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<User>(request.Id);

            LogTryServiceRequest<User>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<User>(request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted) LogServiceRequestSuccess<User>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<User>(request.Id);
        }
    }
}
