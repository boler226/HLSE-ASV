using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;
using FluentValidation;
using MediatR;

namespace CompanyService.Application.Commands.UpdateCompany {
    public class UpdateCompanyCommandHandler(
        ICompanyRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateCompanyCommand> validator
        ) : IRequestHandler<UpdateCompanyCommand> {
        public async Task Handle(UpdateCompanyCommand requst, CancellationToken cancellationToken) {
            await validator.ValidateAndThrowAsync(requst, cancellationToken);
            var company = await repository.GetByIdAsync(requst.Id, cancellationToken);

            if (company is null)
                throw new Exception("Company not found!");

            company.Name = requst.Name;
            company.Email = requst.Email;
            company.Address = requst.Address;

            await repository.UpdateAsync(company, cancellationToken);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
