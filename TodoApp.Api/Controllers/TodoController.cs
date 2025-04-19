using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TodoController(IMediator mediator) : ControllerBase
{
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] GetAllTodoRequest getAllTodoRequest)
    {
        var result = await mediator.Send(getAllTodoRequest);
        return Ok(ApiResponse<PaginationResponse<GetAllTodoResponse>>.SuccessResponse(result));
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> Get([FromQuery] GetTodoRequest getTodoRequest)
    {
        var result = await mediator.Send(getTodoRequest);
        return Ok(ApiResponse<GetTodoResponse>.SuccessResponse(result));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse>> Status([FromBody] ChangeStatusTodoRequest changeStatusTodoRequest)
    {
        await mediator.Send(changeStatusTodoRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<ApiResponse>> Delete([FromBody] DeleteTodoRequest deleteTodoRequest)
    {
        await mediator.Send(deleteTodoRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
}