using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddAuthorCommand(ModelStateDictionary ModelState, AuthorDTO_Request AuthorDTO) : IRequest<AuthorDTO_Request>;
}
