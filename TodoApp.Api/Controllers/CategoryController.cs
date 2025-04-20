using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common.ResponseHanlder;

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
        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateCategory(UpdateCategoryRequest updateCategoryRequest)
    {
        await mediator.Send(updateCategoryRequest);
        return Ok();
    }
}