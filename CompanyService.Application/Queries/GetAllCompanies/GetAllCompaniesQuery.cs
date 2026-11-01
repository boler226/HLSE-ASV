using CompanyService.Application.Behaviors;
using CompanyService.Application.DTOs;
using MediatR;

namespace CompanyService.Application.Queries.GetAllCompanies {
    public record GetAllCompaniesQuery : IRequest<List<CompanyDto>>, ICachedQuery
    {
        public string CacheKey => "companies:all";
    }
}
