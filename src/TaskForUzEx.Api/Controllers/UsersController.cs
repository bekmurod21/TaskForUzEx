using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskForUzEx.Api.WebSockets;
using TaskForUzEx.Application.UseCases.AboutUsers.Commands;
using TaskForUzEx.Application.UseCases.AboutUsers.Queries;

namespace TaskForUzEx.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator, WebSocketHandler webSocketHandler) : ControllerBase
    {
        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand command)
        {
            var user = await mediator.Send(command);
            if (user is null)
            {
                return NotFound(new { message = "User not found" });
            }

            string message = $"User {user.UserName} deleted";
            await webSocketHandler.NotifyClients(message);

            return Ok(message);
        }
        
        [HttpGet("get-all-users"),Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUserQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("me"), Authorize]
        public async Task<IActionResult> GetUserByToken()
        {
            var result = await mediator.Send(new GetUserByTokenQuery());
            return Ok(result);
        }
    }
}
