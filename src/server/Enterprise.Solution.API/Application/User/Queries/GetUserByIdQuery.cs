using MediatR;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetUserByIdQuery(int Id) : IRequest<UserDTO_Response>;
}
