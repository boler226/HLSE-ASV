using MediatR;

namespace UserService.Application.Commands.UpdateUser {
    public record UpdateUserCommand(Guid Id, string FirstName, string LastName, string Email, Guid CompanyId) : IRequest;
}
