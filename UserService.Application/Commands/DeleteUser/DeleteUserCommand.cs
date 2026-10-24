using MediatR;

namespace UserService.Application.Commands.DeleteUser {
    public record DeleteUserCommand(Guid Id) : IRequest;
}
