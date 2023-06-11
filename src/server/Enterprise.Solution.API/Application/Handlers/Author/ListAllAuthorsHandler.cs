using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for ListAll Query
    /// </summary>
    public class ListAllAuthorsHandler : BaseHandler<ListAllAuthorsHandler>, IRequestHandler<ListAllAuthorsQuery, EntityListWithPaginationMetadata<Author>>
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
        public ListAllAuthorsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListAllAuthorsHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _logger.LogInformation($"{nameof(ListAllAuthorsHandler)} constructor invoked.");

            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<Author>> Handle(ListAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ListAllAuthorsHandler)} handler invoked with request type {nameof(ListAllAuthorsQuery)}.");

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            _logger.LogInformation($"Try to list all of type {nameof(Author)}");
            return await _service.ListAllAsync(
                pageNumber,
                pageSize,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeBooks ?? false,
                request.QueryParams.IncludeBooksWithCover ?? false,
                request.QueryParams.IncludeBooksWithCoverAndArtists ?? false);
        }
    }
}
