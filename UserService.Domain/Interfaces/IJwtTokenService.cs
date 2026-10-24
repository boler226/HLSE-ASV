using UsersService.Domain.Entities;

namespace UsersService.Domain.Interfaces {
    public interface IJwtTokenService {
        Task<string> GenerateTokenAsync(User user);
    }
}
