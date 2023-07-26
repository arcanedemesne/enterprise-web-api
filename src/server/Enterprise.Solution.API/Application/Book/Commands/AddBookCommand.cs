using Enterprise.Solution.API.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddBookCommand(ModelStateDictionary ModelState, BookDTO_Request BookDTO) : IRequest<BookDTO_Request>;
}
