using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Auth.Commands.LoginCommand;
using TodoApp.Application.Auth.Commands.RegisterCommand;
using TodoApp.Common.ResponseHanlder;


namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
// Authorization
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterCommand registerCommand)
    {
        await mediator.Send(registerCommand);
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        var result = await mediator.Send(loginCommand);
        return Ok(ApiResponse<LoginResponse>.SuccessResponse(result));
    }
}