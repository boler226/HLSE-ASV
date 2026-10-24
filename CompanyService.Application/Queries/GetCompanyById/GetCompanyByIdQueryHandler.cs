using AutoMapper;
using CompanyService.Application.DTOs;
using CompanyService.Domain.Interfaces;
using MediatR;

namespace CompanyService.Application.Queries.GetCompanyById {
    public class GetCompanyByIdQueryHandler(
        ICompanyRepository companyRepository,
        IMapper mapper
        ) : IRequestHandler<GetCompanyByIdQuery, CompanyDto> {
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken) {
            var company = await companyRepository.GetByIdAsync(request.id, cancellationToken);

            if (company is null)
                throw new Exception("Company not found!");

            return mapper.Map<CompanyDto>(company);
        }
    }
}
