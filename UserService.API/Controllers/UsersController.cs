using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands.DeleteUser;
using UserService.Application.Commands.UpdateUser;
using UsersService.Application.Commands.CreateUser;
using UsersService.Application.DTOs;
using UsersService.Application.Queries.GetAllUsers;
using UsersService.Application.Queries.GetUserById;
using UsersService.Application.Queries.LoginUser;

namespace UsersService.API.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController(IMediator mediator) : ControllerBase {
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var result = await mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            var result = await mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command) {
            var userId = await mediator.Send(command);
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            var token = await mediator.Send(new LoginUserQuery(loginDto.Email, loginDto.Password));
            return Ok(new { token });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserCommand command) {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            await mediator.Send(new DeleteUserCommand(id));
            return Ok(id);
        }
    }
}
