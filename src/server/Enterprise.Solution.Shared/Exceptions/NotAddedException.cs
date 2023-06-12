using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotAddedException : Exception
    {
        public NotAddedException()
        {
        }

        public NotAddedException(string message)
            : base(message)
        {
        }

        public NotAddedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public NotAddedException(Type entityType) : base($"An entity of type {nameof(entityType)} failed to {RequestType.Add}.") { }
    }
}
