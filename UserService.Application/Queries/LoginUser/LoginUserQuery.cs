using MediatR;

namespace UsersService.Application.Queries.LoginUser {
    public record LoginUserQuery(string Email, string Password) : IRequest<string>;
}
