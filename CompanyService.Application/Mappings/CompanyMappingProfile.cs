using AutoMapper;
using CompanyService.Application.DTOs;
using CompanyService.Domain.Entities;

namespace CompanyService.Application.Mappings {
    public class CompanyMappingProfile : Profile {
        public CompanyMappingProfile() {
            CreateMap<Company, CompanyDto>();
        }
    }
}
