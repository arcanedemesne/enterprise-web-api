using Enterprise.Solution.Data.Helpers;
using Enterprise.Solution.Data.Models;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Base;
using Microsoft.Extensions.Logging;

namespace Enterprise.Solution.Service.Services
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            ILogger<INotificationService> logger,
            IBaseRepository<Notification> baseRepository,
            INotificationRepository notificationRepository
        ) : base(logger, baseRepository)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }


        public async Task<IReadOnlyList<Notification>> ListAllAsync(Guid assignedTo, bool onlyShowDeleted = false)
        {
            return await _notificationRepository.ListAllAsync(assignedTo, onlyShowDeleted);
        }

    }
}
