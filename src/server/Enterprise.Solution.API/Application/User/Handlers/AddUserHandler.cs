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
    public class AddUserHandler : BaseHandler<AddUserHandler>, IRequestHandler<AddUserCommand, UserDTO_Request>
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
        public AddUserHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddUserHandler> logger,
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
        public async Task<UserDTO_Request> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<AddUserCommand>();

            if (!request.ModelState.IsValid) LogAndThrowInvalidModelException<User>(RequestType.Add);

            var entity = _mapper.Map<User>(request.UserDTO);

            LogTryServiceRequest<User>(RequestType.Add);
            var createdEntity = await _service.AddAsync(entity);

            if (createdEntity != null)
            {
                LogServiceRequestSuccess<User>(RequestType.Add, createdEntity.Id);
                return _mapper.Map<UserDTO_Request>(createdEntity);
            }

            LogAndThrowNotAddedException<User>();
            return new UserDTO_Request();
        }
    }
}
