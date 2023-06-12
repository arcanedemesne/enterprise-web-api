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
    public class DeleteAuthorHandler : BaseHandler<DeleteAuthorHandler>, IRequestHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public DeleteAuthorHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteAuthorHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteAuthorCommand>();

            LogCheckIfExists<Author>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Author>(request.Id);

            LogTryServiceRequest<Author>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<Author>(request.Id);
            var stillExists = await _service.ExistsAsync(request.Id);

            if (!stillExists) LogServiceRequestSuccess<Author>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<Author>(request.Id);
        }
    }
}
