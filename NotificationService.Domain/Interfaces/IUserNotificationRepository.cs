using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces {
    public interface IUserNotificationRepository {
        Task AddAsync(UserNotificationSchedule schedule);
        Task<List<UserNotificationSchedule>> GetUsersToNotifyAsync();
        Task MarkAsNotifiedAsync(Guid id);
    }
}
