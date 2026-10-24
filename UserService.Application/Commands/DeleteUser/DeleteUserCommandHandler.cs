using MediatR;
using UsersService.Domain.Interfaces;
using UsersService.Infrastructure.UnitOfWork.Interfaces;

namespace UserService.Application.Commands.DeleteUser {
    public class DeleteUserCommandHandler(
        IUnitOfWork unitOfWork
        ) : IRequestHandler<DeleteUserCommand> {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
            var user = await unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
                throw new Exception("User not found!");

            await unitOfWork.Users.DeleteAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
