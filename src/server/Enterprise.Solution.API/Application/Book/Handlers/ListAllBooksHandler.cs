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
    /// Handler for ListAll Query
    /// </summary>
    public class ListAllBooksHandler : BaseHandler<ListAllBooksHandler>, IRequestHandler<ListAllBooksQuery, IReadOnlyList<Book>>
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
        public ListAllBooksHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<ListAllBooksHandler> logger,
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
        public async Task<IReadOnlyList<Book>> Handle(ListAllBooksQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<ListAllBooksQuery>();

            LogTryServiceRequest<Book>(RequestType.ListAll);
            return await _service.ListAllAsync(
                request.QueryParams.SearchQuery,
                request.QueryParams.OnlyShowDeleted ?? false);
        }
    }
}
