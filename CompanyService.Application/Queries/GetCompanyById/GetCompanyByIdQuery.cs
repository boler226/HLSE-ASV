using CompanyService.Application.Behaviors;
using CompanyService.Application.DTOs;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyById {
    public record GetCompanyByIdQuery(Guid Id) : IRequest<CompanyDto>, ICachedQuery
    {
        public string CacheKey => $"company:{Id}";
    }
}
