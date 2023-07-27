using Enterprise.Solution.API.Models;

using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Patch
    /// </summary>
    public record PatchCoverCommand(int Id, ModelStateDictionary ModelState, Func<object, bool> TryValidateModel, JsonPatchDocument<CoverDTO_Request> JsonPatchDocument) : IRequest;
}
