using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddEmailSubscriptionCommand(ModelStateDictionary ModelState, EmailSubscriptionDTO_Request EmailSubscriptionDTO) : IRequest<EmailSubscriptionDTO_Request>;
}
