using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddUserCommand(ModelStateDictionary ModelState, UserDTO_Request UserDTO) : IRequest<UserDTO_Request>;
}
