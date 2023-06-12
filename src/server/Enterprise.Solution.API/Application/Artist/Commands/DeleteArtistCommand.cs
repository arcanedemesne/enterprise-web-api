using MediatR;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Delete
    /// </summary>
    public record DeleteArtistCommand(int Id) : IRequest;
}
