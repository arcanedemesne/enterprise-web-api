namespace Enterprise.Solution.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotFoundException(string entityTypeName, int id) : base($"Entity of type {entityTypeName} with id {id} was not found.") { }
    }
}
