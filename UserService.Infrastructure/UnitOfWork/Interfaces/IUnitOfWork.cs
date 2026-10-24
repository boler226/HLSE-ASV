using UsersService.Domain.Interfaces;

namespace UsersService.Infrastructure.UnitOfWork.Interfaces {
    public interface IUnitOfWork {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
