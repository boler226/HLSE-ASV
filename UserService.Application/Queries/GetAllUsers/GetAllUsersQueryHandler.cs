using AutoMapper;
using MediatR;
using UsersService.Application.DTOs;
using UsersService.Domain.Interfaces;

namespace UsersService.Application.Queries.GetAllUsers {
    internal class GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IMapper mapper
        ) : IRequestHandler<GetAllUsersQuery, List<UserDto>> {
        public async Task<List<UserDto>> Handle(GetAllUsersQuery requst, CancellationToken cancellationToken) {
            var users = await userRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<UserDto>>(users);
        }
    }
}
