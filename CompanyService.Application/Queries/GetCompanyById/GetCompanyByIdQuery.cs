using CompanyService.Application.DTOs;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyById {
    public record GetCompanyByIdQuery(Guid id) : IRequest<CompanyDto>;
}
