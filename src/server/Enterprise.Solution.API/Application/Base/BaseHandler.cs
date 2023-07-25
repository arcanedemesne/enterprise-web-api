using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

using Enterprise.Solution.Shared;
using Enterprise.Solution.Shared.Exceptions;

using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.API.Application
{
    /// <summary>
    /// BaseHandler
    /// </summary>
    public class BaseHandler<THandler> where THandler : BaseHandler<THandler>
    {
        private readonly int DEFAULT_PAGE_SIZE = 10;
        private readonly int MAX_PAGE_SIZE = 25;

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
        protected readonly ILogger<THandler> _logger;

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
            ILogger<THandler> logger,
            IMapper mapper)
        {
            _solutionSettings = solutionSettings.Value;
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;

            LogInsideConstructor();
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

        /// <summary>
        /// This should be called at the start of this constructor
        /// </summary>
        public void LogInsideConstructor()
        {
            _logger.LogInformation($"{nameof(THandler)} constructor invoked.");
        }

        /// <summary>
        /// This is called at the top of the Handle method in a Handler
        /// </summary>
        /// <typeparam name="TQueryOrCommand"></typeparam>
        public void LogInsideHandler<TQueryOrCommand>()
        {
            _logger.LogInformation($"{nameof(THandler)} handler invoked with request type {nameof(TQueryOrCommand)}.");
        }

        /// <summary>
        /// This is called if the model state is invalid
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="requestType"></param>
        public void LogAndThrowInvalidModelException<TEntity>(RequestType requestType)
        {
            var ex = new InvalidModelException(typeof(TEntity), requestType);
            _logger.LogError(ex.Message);
            throw ex;
        }

        /// <summary>
        /// This is called right before checking a service to see if a resource exists
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void LogCheckIfExists<TEntity>(int id)
        {
            _logger.LogInformation($"Check if {nameof(TEntity)} with id {id} exists.");
        }

        /// <summary>
        /// This is called right before making a request to a service
        /// </summary>
        /// <typeparam name="Artist"></typeparam>
        /// <param name="requestType"></param>
        /// <param name="id"></param>
        public void LogTryServiceRequest<Artist>(RequestType requestType, int? id = null)
        {
            _logger.LogInformation($"Try to {requestType} a(n) {nameof(Artist)} {(id.HasValue ? $" with id {id.Value}" : "")}.");
        }

        /// <summary>
        /// This is called when a service request has been confirmed as a success
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="requestType"></param>
        /// <param name="id"></param>
        public void LogServiceRequestSuccess<TEntity>(RequestType requestType, int? id)
        {
            _logger.LogInformation($"{nameof(TEntity)} with id {id} was successfully {requestType}ed.");
        }

        /// <summary>
        /// This is called right after checking a service, and seeing that the resource does not exist
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void LogAndThrowNotFoundException<TEntity>(int id)
        {
            var ex = new NotFoundException(nameof(TEntity), id);
            _logger.LogError(ex.Message);
            throw ex;
        }

        /// <summary>
        /// This is called when the request failed to Add
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public void LogAndThrowNotAddedException<TEntity>()
        {
            var ex = new NotAddedException(typeof(TEntity));
            _logger.LogError(ex.Message);
            throw ex;
        }

        /// <summary>
        /// This is called when the request failed to Update
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void LogAndThrowNotUpdatedException<TEntity>(int id)
        {
            var ex = new NotUpdatedException(typeof(TEntity), id);
            _logger.LogError(ex.Message);
            throw ex;
        }

        /// <summary>
        /// This is called when the request failed to Patch
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void LogAndThrowNotPatchedException<TEntity>(int id)
        {
            var ex = new NotPatchedException(typeof(TEntity), id);
            _logger.LogError(ex.Message);
            throw ex;
        }

        /// <summary>
        /// This is called when the request failed to Delete
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void LogAndThrowNotDeletedException<TEntity>(int id)
        {
            var ex = new NotDeletedException(typeof(TEntity), id);
            _logger.LogError(ex.Message);
            throw ex;
        }
    }
}
