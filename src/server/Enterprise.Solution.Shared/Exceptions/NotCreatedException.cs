using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotCreatedException : Exception
    {
        public NotCreatedException()
        {
        }

        public NotCreatedException(string message)
            : base(message)
        {
        }

        public NotCreatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public NotCreatedException(Type entityType) : base($"An entity of type {nameof(entityType)} failed to {RequestType.Add}.") { }
    }
}
