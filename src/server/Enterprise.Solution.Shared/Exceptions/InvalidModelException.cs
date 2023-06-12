using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(string message)
            : base(message)
        {
        }

        public InvalidModelException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidModelException(Type entityType, RequestType requestType) : base($"An entity of type {nameof(entityType)} attempting to be {requestType}ed has an invalid ModelState.") { }
    }
}
