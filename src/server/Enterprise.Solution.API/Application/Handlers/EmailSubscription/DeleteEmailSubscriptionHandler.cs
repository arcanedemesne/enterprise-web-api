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
    public class DeleteEmailSubscriptionHandler : BaseHandler<DeleteEmailSubscriptionHandler>, IRequestHandler<DeleteEmailSubscriptionCommand>
    {
        private readonly IEmailSubscriptionService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public DeleteEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<DeleteEmailSubscriptionHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(DeleteEmailSubscriptionHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(DeleteEmailSubscriptionHandler)} handler invoked with request type {nameof(DeleteEmailSubscriptionCommand)}.");

            _logger.LogInformation($"Check if {nameof(EmailSubscription)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            _logger.LogInformation($"Try to delete {nameof(EmailSubscription)} with id {request.Id}.");
            await _service.DeleteAsync(request.Id);

            var stillExists = await _service.ExistsAsync(request.Id);
            if (!stillExists)
            {
                _logger.LogInformation($"{nameof(EmailSubscription)} with id {request.Id} was successfully deleted.");
                return;
            }
            else
            {
                var ex = new NotDeletedException(typeof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
