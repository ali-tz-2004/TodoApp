using MediatR;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.CreateTodoCommand;

public class CreateTodoCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    ITodoQueryRepository todoQueryRepository,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<CreateTodoCommand>
{
    public async Task Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        if (!await todoQueryRepository.ExistsCategoryAsync(command.CategoryId, command.UserId, cancellationToken))
            throw new NotFoundException("Category not found");

        var todo = TodoItem.CreateTodo(command.Title, command.Description, command.DueDate, command.CategoryId,
            command.UserId);

        todoCommandRepository.CreateTodo(todo);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}