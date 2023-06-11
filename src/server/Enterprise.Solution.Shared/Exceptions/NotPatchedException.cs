using static Enterprise.Solution.Shared.Exceptions.ExceptionHelper;

namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotPatchedException : Exception
    {
        public NotPatchedException()
        {
        }

        public NotPatchedException(string message)
            : base(message)
        {
        }

        public NotPatchedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotPatchedException(Type entityType, int id) : base($"{nameof(entityType)} with id {id} failed to {RequestType.Patch}.") { }
    }
}
