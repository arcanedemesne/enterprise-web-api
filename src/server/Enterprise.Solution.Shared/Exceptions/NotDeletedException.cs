using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotDeletedException : Exception
    {
        public NotDeletedException()
        {
        }

        public NotDeletedException(string message)
            : base(message)
        {
        }

        public NotDeletedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public NotDeletedException(Type entityType, int id) : base($"{nameof(entityType)} with id {id} failed to {RequestType.Delete}.") { }
    }
}
