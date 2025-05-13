using MediatR;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.DeleteTodoCommand;

public class DeleteTodoCommandHandler(
    ITodoQueryRepository todoQueryRepository,
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTodoCommand>
{
    public async Task Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await todoQueryRepository.GetById(command.Id, command.UserId, cancellationToken);
        todoCommandRepository.DeleteTodo(todo);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}