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

        public InvalidModelException(RequestType requestType) : base($"ModelState {requestType} is invalid.") { }
    }
}
