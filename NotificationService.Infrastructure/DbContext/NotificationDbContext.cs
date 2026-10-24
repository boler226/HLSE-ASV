using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.DbContext {
    public class NotificationDbContext : Microsoft.EntityFrameworkCore.DbContext {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }
        public DbSet<UserNotificationSchedule> UserNotificationSchedules => Set<UserNotificationSchedule>();
    }
}
