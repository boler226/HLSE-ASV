using CompanyService.Application.Commands.CreateCompany;
using FluentValidation;

namespace CompanyService.Application.Validators {
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand> {
        public CreateCompanyValidator() {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.Address).NotEmpty();
        }
    }
}
