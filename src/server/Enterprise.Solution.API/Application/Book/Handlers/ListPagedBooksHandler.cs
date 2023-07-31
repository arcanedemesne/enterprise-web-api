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
    public class ListPagedBooksHandler : BaseHandler<ListPagedBooksHandler>, IRequestHandler<ListPagedBooksQuery, EntityListWithPaginationMetadata<Book>>
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
        public ListPagedBooksHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListPagedBooksHandler> logger,
            IMapper mapper,
            IBookService service) : base(solutionSettings, mediator, logger, mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Handle the Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EntityListWithPaginationMetadata<Book>> Handle(ListPagedBooksQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListPagedBooksQuery>();

            var (pageNumber, pageSize) = ValidatePagedParams(request.QueryParams.PageNumber, request.QueryParams.PageSize);

            LogTryServiceRequest<Book>(RequestType.ListPaged);
            return await _service.ListPagedAsync(
                pageNumber, pageSize,
                request.QueryParams.OrderBy,
                request.QueryParams.SearchQuery,
                request.QueryParams.IncludeAuthor ?? false,
                request.QueryParams.IncludeCover ?? false,
                request.QueryParams.IncludeCoverAndArtists ?? false,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
