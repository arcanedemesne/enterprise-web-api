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
    public class DeleteBookHandler : BaseHandler<DeleteBookHandler>, IRequestHandler<DeleteBookCommand>
    {
        private readonly IBookService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public DeleteBookHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteBookHandler> logger,
            IMapper mapper,
            IBookService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            LogInsideHandler<DeleteBookCommand>();

            LogCheckIfExists<Book>(request.Id);
            var exists = await _service.ExistsAsync(request.Id);
            if (!exists) LogAndThrowNotFoundException<Book>(request.Id);

            LogTryServiceRequest<Book>(RequestType.Delete, request.Id);
            await _service.DeleteAsync(request.Id);

            LogCheckIfExists<Book>(request.Id);
            var stillExists = await _service.ExistsAsync(request.Id);

            if (!stillExists) LogServiceRequestSuccess<Book>(RequestType.Delete, request.Id);
            else LogAndThrowNotDeletedException<Book>(request.Id);
        }
    }
}
