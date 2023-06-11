using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotUpdatedException : Exception
    {
        public NotUpdatedException()
        {
        }

        public NotUpdatedException(string message)
            : base(message)
        {
        }

        public NotUpdatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public NotUpdatedException(Type entityType, int id) : base($"{nameof(entityType)} with id {id} failed to {RequestType.Update}.") { }
    }
}
