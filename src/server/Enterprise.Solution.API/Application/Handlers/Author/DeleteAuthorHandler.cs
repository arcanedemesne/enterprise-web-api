using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

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
            _logger.LogInformation($"{nameof(DeleteAuthorHandler)} constructor invoked.");

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
            _logger.LogInformation($"{nameof(DeleteAuthorHandler)} handler invoked with request type {nameof(DeleteAuthorCommand)}.");

            _logger.LogInformation($"Check if {nameof(Author)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            _logger.LogInformation($"Try to delete {nameof(Author)} with id {request.Id}.");
            await _service.DeleteAsync(request.Id);

            var stillExists = await _service.ExistsAsync(request.Id);
            if (!stillExists)
            {
                _logger.LogInformation($"{nameof(Author)} with id {request.Id} was successfully deleted.");
                return;
            }
            else
            {
                var ex = new NotDeletedException(typeof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
