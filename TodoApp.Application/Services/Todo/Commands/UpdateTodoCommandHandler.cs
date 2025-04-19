using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class UpdateTodoCommandHandler(ITodoCommandRepository todoCommandRepository, ITodoQueryRepository todoQueryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateTodoRequest>
{
    public async Task Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        if (!await todoQueryRepository.ExistsCategoryAsync(request.CategoryId, request.UserId, cancellationToken))
            throw new NotFoundException("Category not found");
        
        var todoItem = await todoQueryRepository.GetById(request.Id, cancellationToken);

        todoItem.UpdateTodo(request.Title, request.Description, request.DueDate, request.CategoryId, request.UserId);
        
        todoCommandRepository.UpdateTodo(todoItem);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}