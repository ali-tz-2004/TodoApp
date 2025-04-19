using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common.Exceptions;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    // Authorization
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateTodo([FromBody] CreateTodoRequest createTodoRequest)
    {
        await mediator.Send(createTodoRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdateTodo([FromBody] UpdateTodoRequest updateTodoRequest)
    {
        await mediator.Send(updateTodoRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
}