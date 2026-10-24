using AutoMapper;
using CompanyService.Application.DTOs;
using CompanyService.Domain.Interfaces;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyByEmail {
    public class GetCompanyByEmailQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper
        ) : IRequestHandler<GetCompanyByEmailQuery, CompanyDto> {
        public async Task<CompanyDto> Handle(GetCompanyByEmailQuery request, CancellationToken cancellationToken) {
            var company = await companyRepository.GetByEmailAsync(request.email, cancellationToken);

            if (company is null)
                throw new Exception("Company not found!");

            return mapper.Map<CompanyDto>(company);
        }
    }
}
