using MediatR;
using UsersService.Application.DTOs;

namespace UsersService.Application.Queries.GetUserById {
    public record GetUserByIdQuery(Guid id) : IRequest<UserDto>;
}
