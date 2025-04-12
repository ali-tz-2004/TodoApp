using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class CreateTodoCommandHandler(ITodoCommandRepository todoCommandRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTodoRequest>
{
    public async Task Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = TodoItem.CreateTodo(request.Title, request.Description, request.DueDate, request.CategoryId,
            request.UserId);

        todoCommandRepository.CreateTodo(todo);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}