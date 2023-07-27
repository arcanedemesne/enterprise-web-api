﻿using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Patch
    /// </summary>
    public record PatchAuthorCommand(int Id, ModelStateDictionary ModelState, Func<object, bool> TryValidateModel, JsonPatchDocument<AuthorDTO_Request> JsonPatchDocument) : IRequest;
}
