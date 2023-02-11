using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Solution.API.Helpers
{
    /// <summary>
    /// Filter for handling the CancellationToken
    /// </summary>
    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for OperationCancelledExceptionFilter
        /// </summary>
        /// <param name="loggerFactory"></param>
        public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
        }

        /// <summary>
        /// OnException handles the cancelled request
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(400);
            }
        }
    }
}
