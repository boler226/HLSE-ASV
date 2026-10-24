using FluentValidation;
using MediatR;
using UsersService.Infrastructure.UnitOfWork.Interfaces;

namespace UserService.Application.Commands.UpdateUser {
    public class UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<UpdateUserCommand> validator
        ) : IRequestHandler<UpdateUserCommand> {
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var user = await unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
                throw new Exception("User not found!");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.CompanyId = request.CompanyId;


            await unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
