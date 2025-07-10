using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskForUzEx.Application.UseCases.AboutUsers.Commands;

namespace TaskForUzEx.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInUserCommand command,CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command,cancellationToken);
        return Ok(result);
    }
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpUserCommand command,CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command,cancellationToken);
        if (result)
            return Ok(result);

        return BadRequest();
    }
    [HttpPost("refresh-token"),Authorize]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (result is null)
            return Unauthorized();
        
        return Ok(result);
    }
    [HttpGet("is-empty-email")]
    public async Task<IActionResult> IsEmptyEmailAsync([FromQuery] IsEmptyEmailCommand command,CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command,cancellationToken);
        return Ok(result);
    }
    [HttpGet("is-empty-username")]
    public async Task<IActionResult> IsEmptyUsernameAsync([FromQuery] IsEmptyUserNameCommand command,CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command,cancellationToken);
        return Ok(result);
    }
        
}