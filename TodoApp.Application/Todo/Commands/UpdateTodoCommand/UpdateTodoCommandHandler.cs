using MediatR;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.UpdateTodoCommand;

public class UpdateTodoCommandHandler(ITodoCommandRepository todoCommandRepository, ITodoQueryRepository todoQueryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateTodoCommand>
{
    public async Task Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        if (!await todoQueryRepository.ExistsCategoryAsync(command.CategoryId, command.UserId, cancellationToken))
            throw new NotFoundException("Category not found");
        
        var todoItem = await todoQueryRepository.GetById(command.Id, command.UserId, cancellationToken);

        todoItem.UpdateTodo(command.Title, command.Description, command.DueDate, command.CategoryId, command.UserId);
        
        todoCommandRepository.UpdateTodo(todoItem);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}