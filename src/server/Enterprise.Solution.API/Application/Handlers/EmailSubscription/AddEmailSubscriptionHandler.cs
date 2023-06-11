using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Commands;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Add Command
    /// </summary>
    public class AddEmailSubscriptionHandler : BaseHandler<AddEmailSubscriptionHandler>, IRequestHandler<AddEmailSubscriptionCommand, EmailSubscriptionDTO>
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
        public AddEmailSubscriptionHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<AddEmailSubscriptionHandler> logger,
            IMapper mapper,
            IEmailSubscriptionService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(AddEmailSubscriptionHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        public async Task<EmailSubscriptionDTO> Handle(AddEmailSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(AddEmailSubscriptionHandler)} handler invoked with request type {nameof(AddEmailSubscriptionCommand)}.");

            if (!request.ModelState.IsValid)
            {
                var ex = new InvalidModelException(RequestType.Add);
                _logger.LogError(ex.Message);
                throw ex;
            }

            var emailSubscription = _mapper.Map<EmailSubscription>(request.EmailSubscriptionDTO);

            _logger.LogInformation($"Try to add a {nameof(EmailSubscription)}.");
            var createdEmailSubscription = await _service.AddAsync(emailSubscription);

            if (createdEmailSubscription != null)
            {
                _logger.LogInformation($"{nameof(EmailSubscription)} with id {createdEmailSubscription.Id} was successfully added.");
                return _mapper.Map<EmailSubscriptionDTO>(createdEmailSubscription);
            }
            else
            {
                var ex = new NotCreatedException(typeof(EmailSubscription));
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
