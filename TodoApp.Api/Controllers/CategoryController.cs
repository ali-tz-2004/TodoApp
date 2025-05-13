using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todo.Commands.CreateCategoryCommand;
using TodoApp.Application.Todo.Commands.DeleteCategoryCommand;
using TodoApp.Application.Todo.Commands.UpdateCategoryCommand;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateCategory(CreateCategoryCommand createCategoryCommand)
    {
        await mediator.Send(createCategoryCommand);
        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateCategory(UpdateCategoryCommand updateCategoryCommand)
    {
        await mediator.Send(updateCategoryCommand);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteCategory(DeleteCategoryCommand deleteCategoryCommand)
    {
        await mediator.Send(deleteCategoryCommand);
        return Ok();
    }
}