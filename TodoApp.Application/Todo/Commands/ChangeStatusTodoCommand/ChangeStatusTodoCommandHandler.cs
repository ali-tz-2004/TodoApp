using MediatR;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.ChangeStatusTodoCommand;

public class ChangeStatusTodoCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    ITodoQueryRepository todoQueryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeStatusTodoCommand>
{
    public async Task Handle(ChangeStatusTodoCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await todoQueryRepository.GetById(command.Id, command.UserId, cancellationToken);
        todoItem.ChangeStatus(!todoItem.IsCompleted);
        
        todoCommandRepository.ChangeStatus(todoItem);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}