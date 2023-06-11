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
            _logger.LogInformation($"{nameof(DeleteBookHandler)} constructor invoked.");

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
            _logger.LogInformation($"{nameof(DeleteBookHandler)} handler invoked with request type {nameof(DeleteBookCommand)}.");

            _logger.LogInformation($"Check if {nameof(Book)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(Book), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            _logger.LogInformation($"Try to delete {nameof(Book)} with id {request.Id}.");
            await _service.DeleteAsync(request.Id);

            var stillExists = await _service.ExistsAsync(request.Id);
            if (!stillExists)
            {
                _logger.LogInformation($"{nameof(Book)} with id {request.Id} was successfully deleted.");
                return;
            }
            else
            {
                var ex = new NotDeletedException(typeof(Book), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
