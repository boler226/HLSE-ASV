using MediatR;
using UsersService.Application.DTOs;

namespace UsersService.Application.Queries.GetAllUsers {
    public record GetAllUsersQuery : IRequest<List<UserDto>>;
}
