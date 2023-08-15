using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Commands
{
    /// <summary>
    /// Command to Add
    /// </summary>
    public record AddNotificationCommand(ModelStateDictionary ModelState, NotificationDTO_Request NotificationDTO) : IRequest<NotificationDTO_Request>;
}
