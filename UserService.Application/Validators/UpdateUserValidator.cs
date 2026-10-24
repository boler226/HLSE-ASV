using FluentValidation;
using UserService.Application.Commands.UpdateUser;
using UserService.Domain.Interfaces;

namespace UserService.Application.Validators {
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand> {
        public UpdateUserValidator(IEntityExistenceChecker checker) {
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();

            RuleFor(u => u.CompanyId)
                .MustAsync(checker.CompanyExistsAsync)
                .WithMessage("Компанія з таким ID не існує.");
        }
    }
}
