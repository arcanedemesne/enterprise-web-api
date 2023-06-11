using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Get By Id Query
    /// </summary>
    public class GetEmailSubscriptionByIdHandler : BaseHandler<GetEmailSubscriptionByIdHandler>, IRequestHandler<GetEmailSubscriptionByIdQuery, EmailSubscriptionDTO>
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
        public GetEmailSubscriptionByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetEmailSubscriptionByIdHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(GetEmailSubscriptionByIdHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmailSubscriptionDTO> Handle(GetEmailSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetEmailSubscriptionByIdHandler)} handler invoked with request type {nameof(GetEmailSubscriptionByIdQuery)}.");

            _logger.LogInformation($"Try to get a {nameof(EmailSubscription)} with id {request.Id}.");
            var emailSubscription = await _service.GetByIdAsync(request.Id);

            if (emailSubscription == null)
            {
                var ex = new NotFoundException(nameof(EmailSubscription), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            return _mapper.Map<EmailSubscriptionDTO>(emailSubscription);
        }
    }
}
