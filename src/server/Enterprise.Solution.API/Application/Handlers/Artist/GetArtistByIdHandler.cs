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
    public class GetArtistByIdHandler : BaseHandler<GetArtistByIdHandler>, IRequestHandler<GetArtistByIdQuery, ArtistDTO>
    {
        private readonly IArtistService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="service"></param>
        public GetArtistByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetArtistByIdHandler> logger,
            IMapper mapper,
            IArtistService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(GetArtistByIdHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ArtistDTO> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetArtistByIdHandler)} handler invoked with request type {nameof(GetArtistByIdQuery)}.");

            _logger.LogInformation($"Try to get a {nameof(Artist)} with id {request.Id}.");
            var emailSubscription = await _service.GetByIdAsync(request.Id);

            if (emailSubscription == null)
            {
                var ex = new NotFoundException(nameof(Artist), request.Id);
                _logger.LogError(ex.Message);
                throw ex;
            }

            return _mapper.Map<ArtistDTO>(emailSubscription);
        }
    }
}
