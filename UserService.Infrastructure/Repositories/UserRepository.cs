using Microsoft.EntityFrameworkCore;
using UsersService.Domain.Entities;
using UsersService.Domain.Interfaces;
using UsersService.Infrastructure.DbContext;

namespace UsersService.Infrastructure.Repositories {
    public class UserRepository(UserDbContext context) : IUserRepository {
        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken) =>
            await context.Users.ToListAsync(cancellationToken);

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task AddAsync(User user, CancellationToken cancellationToken) {
            await context.Users.AddAsync(user, cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken) {
            context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user, CancellationToken cancellationToken) {
            context.Users.Remove(user);
            return Task.CompletedTask;
        }
    }
}
