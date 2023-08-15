using MediatR;

using Enterprise.Solution.API.Models;

namespace Enterprise.Solution.API.Application.Queries
{
    /// <summary>
    /// Query to get by Id
    /// </summary>
    public record GetNotificationByIdQuery(int Id) : IRequest<NotificationDTO_Response>;
}
