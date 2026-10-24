using AutoMapper;
using UsersService.Application.DTOs;
using UsersService.Domain.Entities;

namespace UsersService.Application.Mappings {
    public class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<User, UserDto>();
        }
    }
}
