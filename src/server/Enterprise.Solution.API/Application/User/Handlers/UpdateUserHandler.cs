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
    public class UpdateUserHandler : BaseHandler<UpdateUserHandler>, IRequestHandler<UpdateUserCommand>
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
        public UpdateUserHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateUserHandler> logger,
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
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<UpdateUserCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<User>(RequestType.Update);

            LogCheckIfExists<User>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<User>(request.Id);

            if (!request.Id.Equals(request.UserDTO.Id))
                LogAndThrowNotUpdatedException<User>(request.Id);

            LogTryServiceRequest<User>(RequestType.GetById, request.Id);
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity == null) LogAndThrowNotUpdatedException<User>(request.Id);
            _mapper.Map(request.UserDTO, entity);

            try
            {
                LogTryServiceRequest<User>(RequestType.Update, request.Id);
                await _service.UpdateAsync(entity!);
                return;
            }
            catch (Exception)
            {
                LogAndThrowNotUpdatedException<User>(request.Id);
            }
        }
    }
}
