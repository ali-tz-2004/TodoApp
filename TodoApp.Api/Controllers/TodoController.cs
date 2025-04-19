using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Common.DtoHandler;
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] GetAllTodoRequest getAllTodoRequest)
    {
        var result = await mediator.Send(getAllTodoRequest);
        return Ok(ApiResponse<PaginationResponse<GetAllTodoResponse>>.SuccessResponse(result));
    }
}