using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddAuthorCommand(ModelStateDictionary ModelState, AuthorDTO AuthorDTO) : IRequest<AuthorDTO>;
}
