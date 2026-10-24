using CompanyService.Application.Commands.UpdateCompany;
using FluentValidation;

namespace CompanyService.Application.Validators {
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand> {
        public UpdateCompanyValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.Address).NotEmpty();
        }
    }
}
