using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;
using MediatR;

namespace CompanyService.Application.Commands.DeleteCompany {
    public class DeleteCompanyCommandHandler(
        ICompanyRepository repository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<DeleteCompanyCommand> {
        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken) {
            var company = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (company is null)
                throw new Exception("Company not found!");

            await repository.DeleteAsync(company, cancellationToken);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
