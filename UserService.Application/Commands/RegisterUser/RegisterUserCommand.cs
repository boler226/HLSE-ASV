using MediatR;

namespace UsersService.Application.Commands.CreateUser
{
    public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Guid>;
}
