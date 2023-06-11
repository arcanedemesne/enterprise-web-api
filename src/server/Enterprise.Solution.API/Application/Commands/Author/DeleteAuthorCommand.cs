using MediatR;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Delete
    /// </summary>
    public record DeleteAuthorCommand(int Id) : IRequest;
}
