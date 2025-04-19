using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class DeleteTodoCommandHandler(
    ITodoQueryRepository todoQueryRepository,
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTodoRequest>
{
    public async Task Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = await todoQueryRepository.GetById(request.Id, request.UserId, cancellationToken);
        todoCommandRepository.DeleteTodo(todo);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}