using MediatR;
using UsersService.Domain.Interfaces;
using UsersService.Infrastructure.UnitOfWork.Interfaces;

namespace UsersService.Application.Queries.LoginUser {
    public class LoginUserQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService
        ) : IRequestHandler<LoginUserQuery, string> {
        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken) {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken)
                    ?? throw new Exception("Invalid credentials");
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

           return await jwtTokenService.GenerateTokenAsync(user);
        }
    }
}
