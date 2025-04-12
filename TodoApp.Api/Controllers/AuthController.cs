using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Auth.Request;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        await mediator.Send(registerRequest);
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await mediator.Send(loginRequest);
        return Ok(result);
    }
}