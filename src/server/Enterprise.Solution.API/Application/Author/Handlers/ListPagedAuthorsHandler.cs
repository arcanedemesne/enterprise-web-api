using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for ListPaged Query
    /// </summary>
    public class ListPagedAuthorsHandler : BaseHandler<ListPagedAuthorsHandler>, IRequestHandler<ListPagedAuthorsQuery, EntityListWithPaginationMetadata<Author>>
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
        public ListPagedAuthorsHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListPagedAuthorsHandler> logger,
            IMapper mapper,
            IAuthorService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<Author>> Handle(ListPagedAuthorsQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListPagedAuthorsQuery>();

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            LogTryServiceRequest<Author>(RequestType.ListPaged);
            return await _service.ListPagedAsync(
                pageNumber, pageSize,
                request.QueryParams.OrderBy,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeBooks ?? false,
                request.QueryParams.IncludeBooksWithCover ?? false,
                request.QueryParams.IncludeBooksWithCoverAndArtists ?? false,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
