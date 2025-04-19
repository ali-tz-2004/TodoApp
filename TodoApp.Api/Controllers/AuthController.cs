using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Dto.Auth.Response;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        await mediator.Send(registerRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
    
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await mediator.Send(loginRequest);
        return Ok(ApiResponse<LoginResponse>.SuccessResponse(result));
    }
}