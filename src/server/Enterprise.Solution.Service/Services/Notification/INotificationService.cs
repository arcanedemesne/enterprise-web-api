using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Service.Base;

namespace Enterprise.Solution.Service.Services
{
    public interface INotificationService : IBaseService<Notification>
    {
        Task<IReadOnlyList<Notification>> ListAllAsync(Guid assignedTo, bool onlyShowDeleted);
    }
}
