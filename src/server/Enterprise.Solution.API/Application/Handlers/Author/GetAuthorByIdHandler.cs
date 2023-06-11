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
    public class GetAuthorByIdHandler : BaseHandler<GetAuthorByIdHandler>, IRequestHandler<GetAuthorByIdQuery, AuthorDTO>
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
        public GetAuthorByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetAuthorByIdHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(GetAuthorByIdHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthorDTO> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetAuthorByIdHandler)} handler invoked with request type {nameof(GetAuthorByIdQuery)}.");

            _logger.LogInformation($"Try to get a {nameof(Author)} with id {request.Id}.");
            var emailSubscription = await _service.GetByIdAsync(request.Id);

            if (emailSubscription == null)
            {
                var ex = new NotFoundException(nameof(Author), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            return _mapper.Map<AuthorDTO>(emailSubscription);
        }
    }
}
