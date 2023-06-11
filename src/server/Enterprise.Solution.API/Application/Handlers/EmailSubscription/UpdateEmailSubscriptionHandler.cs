using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Update Command
    /// </summary>
    public class UpdateEmailSubscriptionHandler : BaseHandler<UpdateEmailSubscriptionHandler>, IRequestHandler<UpdateEmailSubscriptionCommand>
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
        public UpdateEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<UpdateEmailSubscriptionHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(UpdateEmailSubscriptionHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(UpdateEmailSubscriptionHandler)} handler invoked with request type {nameof(UpdateEmailSubscriptionCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Update);
                _logger.LogError(ex.Message, ex.InnerException);
                throw ex;
            }

            _logger.LogInformation($"Check if {nameof(EmailSubscription)} with id {request.Id} exists.");
            var exists = await _service.ExistsAsync(request.Id);

            if (!exists)
            {
                var ex = new NotFoundException(nameof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            if (!request.Id.Equals(request.EmailSubscriptionDTO.Id))
            {
                var message = $"Incorrect id provided for {nameof(EmailSubscription)} with id {request.Id}.";
                _logger.LogError(message);
                throw new NotUpdatedException(message);
            }

            _logger.LogInformation($"Try to get {nameof(EmailSubscription)} entity with id {request.Id}.");
            var entity = await _service.GetByIdAsync(request.Id);

            if (entity != null)
            {
                _mapper.Map(request.EmailSubscriptionDTO, entity);

                try
                {
                    _logger.LogInformation($"Try to update a {nameof(EmailSubscription)} with id {request.Id}.");
                    await _service.UpdateAsync(entity);
                }
                catch (Exception ex)
                {
                    var nEx = new NotUpdatedException(typeof(EmailSubscription), request.Id);
                    var fEx = new NotUpdatedException(nEx.Message, ex);
                    _logger.LogError(fEx.Message, fEx.InnerException);
                    throw fEx;
                }
                return;
            }
            else
            {
                var ex = new NotUpdatedException(typeof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
