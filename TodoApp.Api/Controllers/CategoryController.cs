using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common.ResponseHanlder;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]

public class CategoryController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateCategory(CreateCategoryRequest createCategoryRequest)
    {
        await mediator.Send(createCategoryRequest);
        return Ok(ApiResponse.SuccessResponse());
    }
}