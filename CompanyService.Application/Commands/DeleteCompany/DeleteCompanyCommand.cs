using MediatR;

namespace CompanyService.Application.Commands.DeleteCompany {
    public record DeleteCompanyCommand(Guid Id) : IRequest;
}
