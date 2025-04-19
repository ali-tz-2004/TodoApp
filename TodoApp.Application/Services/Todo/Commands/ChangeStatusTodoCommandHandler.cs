using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class ChangeStatusTodoCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    ITodoQueryRepository todoQueryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeStatusTodoRequest>
{
    public async Task Handle(ChangeStatusTodoRequest request, CancellationToken cancellationToken)
    {
        var todoItem = await todoQueryRepository.GetById(request.Id, request.UserId, cancellationToken);
        todoItem.ChangeStatus(request.isCompleted);
        
        todoCommandRepository.ChangeStatus(todoItem);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}