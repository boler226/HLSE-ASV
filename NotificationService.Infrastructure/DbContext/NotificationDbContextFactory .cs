using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotificationService.Infrastructure.DbContext {
    public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext> {
        public NotificationDbContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=UserService;Username=postgres;Password=boler2020");
            return new NotificationDbContext(optionsBuilder.Options);
        }
    }
}
