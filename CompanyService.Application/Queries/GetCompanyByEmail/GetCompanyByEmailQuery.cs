using CompanyService.Application.DTOs;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyByEmail {
    public record GetCompanyByEmailQuery(string email) : IRequest<CompanyDto>;
}
