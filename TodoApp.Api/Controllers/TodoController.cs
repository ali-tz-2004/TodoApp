using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todo.Commands.ChangeStatusTodoCommand;
using TodoApp.Application.Todo.Commands.CreateTodoCommand;
using TodoApp.Application.Todo.Commands.DeleteTodoCommand;
using TodoApp.Application.Todo.Commands.UpdateTodoCommand;
using TodoApp.Application.Todo.Queries.GetAllTodoQuery;
using TodoApp.Application.Todo.Queries.GetTodoQuery;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TodoController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateTodo([FromBody] CreateTodoCommand createTodoCommand)
    {
        await mediator.Send(createTodoCommand);
        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdateTodo([FromBody] UpdateTodoCommand updateTodoCommand)
    {
        await mediator.Send(updateTodoCommand);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] GetAllTodoQuery getAllTodoQuery)
    {
        var result = await mediator.Send(getAllTodoQuery);
        return Ok(ApiResponse<PaginationResponse<GetAllTodoResponse>>.SuccessResponse(result));
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> Get([FromQuery] GetTodoQuery getTodoQuery)
    {
        var result = await mediator.Send(getTodoQuery);
        return Ok(ApiResponse<GetTodoResponse>.SuccessResponse(result));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse>> ChangeStatus([FromBody] ChangeStatusTodoCommand changeStatusTodoCommand)
    {
        await mediator.Send(changeStatusTodoCommand);
        return Ok(ApiResponse.SuccessResponse());
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<ApiResponse>> Delete([FromBody] DeleteTodoCommand deleteTodoCommand)
    {
        await mediator.Send(deleteTodoCommand);
        return Ok(ApiResponse.SuccessResponse());
    }
}