using Enterprise.Solution.Shared;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace Enterprise.Solution.API.Application.Handlers
{
    /// <summary>
    /// BaseHandler
    /// </summary>
    public class BaseHandler<T> where T : BaseHandler<T>
    {
        private readonly int DEFAULT_PAGE_SIZE = 10;
        private readonly int MAX_PAGE_SIZE = 20;

        /// <summary>
        /// SolutionSettings from the appsettings.json file
        /// </summary>
        protected SolutionSettings _solutionSettings;

        /// <summary>
        /// IMediator for mediting api requests, validation, and service actions
        /// </summary>
        protected readonly IMediator _mediator;

        /// <summary>
        /// ILogger for logging
        /// </summary>
        protected readonly ILogger<T> _logger;

        /// <summary>
        /// IMapper for mapping entities and dtos
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Constructor for the BaseHandler with MediatR
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public BaseHandler(
            IOptions<SolutionSettings> solutionSettings,
            IMediator mediator,
            ILogger<T> logger,
            IMapper mapper)
        {
            _solutionSettings = solutionSettings.Value;
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Validates Paged Params and sets defaul values if values are null
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<int, int> ValidatePagedParams(int? pageNumber, int? pageSize)
        {
            var PageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            if (PageNumber < 1) PageNumber = 1;

            var PageSize = pageSize.HasValue ? pageSize.Value : DEFAULT_PAGE_SIZE;
            if (PageSize < 1 || PageSize > MAX_PAGE_SIZE) PageSize = DEFAULT_PAGE_SIZE;

            return Tuple.Create(PageNumber, PageSize);
        }
    }
}
