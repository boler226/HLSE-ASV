using UsersService.Domain.Interfaces;
using UsersService.Infrastructure.DbContext;
using UsersService.Infrastructure.UnitOfWork.Interfaces;

namespace UsersService.Infrastructure.UnitOfWork {
    public class UnitOfWork(
        UserDbContext context,
        IUserRepository userRepository
        ) : IUnitOfWork {
        public IUserRepository Users => userRepository;
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await context.SaveChangesAsync(cancellationToken);
    }
}
