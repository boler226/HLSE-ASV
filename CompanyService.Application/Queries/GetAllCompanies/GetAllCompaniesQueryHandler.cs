using AutoMapper;
using CompanyService.Application.DTOs;
using CompanyService.Domain.Interfaces;
using MediatR;

namespace CompanyService.Application.Queries.GetAllCompanies {
    public class GetAllCompaniesQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>> {
        public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken) {
            var companies = await companyRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<CompanyDto>>(companies);
        }
    }
}
