using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.API.Application.Queries;
using Enterprise.Solution.API.Models;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Shared;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// Handler for Get By Id Query
    /// </summary>
    public class GetBookByIdHandler : BaseHandler<GetBookByIdHandler>, IRequestHandler<GetBookByIdQuery, BookDTO_Response>
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
        public GetBookByIdHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<GetBookByIdHandler> logger,
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
        public async Task<BookDTO_Response> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            LogInsideHandler<GetBookByIdQuery>();

            LogTryServiceRequest<Book>(RequestType.GetById, request.id);
            var entity = await _service.GetByIdAsync(
                request.id,
                request.QueryParams.IncludeAuthor ?? false,
                request.QueryParams.IncludeCover ?? false,
                request.QueryParams.IncludeCoverAndArtists ?? false);

            if (entity == null) LogAndThrowNotFoundException<Book>(request.id);

            return _mapper.Map<BookDTO_Response>(entity);
        }
    }
}
