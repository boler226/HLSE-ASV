using CompanyService.Domain.Entities;
using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.UnitOfWork.Interfaces;
using FluentValidation;
using MassTransit;
using MediatR;

namespace CompanyService.Application.Commands.CreateCompany {
    public class CreateCompanyCommandHandler(
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint,
        IValidator<CreateCompanyCommand> validator
        ) : IRequestHandler<CreateCompanyCommand, Guid> {
        public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken) {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var company = new Company {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.Companies.AddAsync(company, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            return company.Id;
        }
    }
}
