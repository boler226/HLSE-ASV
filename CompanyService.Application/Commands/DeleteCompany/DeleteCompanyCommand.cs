using CompanyService.Application.Behaviors;
using MediatR;

namespace CompanyService.Application.Commands.DeleteCompany {
    public record DeleteCompanyCommand(Guid Id) : IRequest, ICachedQuery
    {
        public string CacheKey => $"company:{Id}";
    }
}
