using CompanyService.Application.DTOs;
using MediatR;

namespace CompanyService.Application.Queries.GetAllCompanies {
    public record GetAllCompaniesQuery : IRequest<List<CompanyDto>>;
}
