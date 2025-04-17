using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    // Authorization
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateTodo([FromBody] CreateTodoRequest createTodoRequest)
    {
        await mediator.Send(createTodoRequest);
        return Ok();
    }
}