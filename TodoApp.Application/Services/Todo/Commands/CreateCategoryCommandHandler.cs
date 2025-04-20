using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class CreateCategoryCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCategoryRequest>
{
    public async Task Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = Category.CreateCategory(request.Name, request.UserId);
        
        todoCommandRepository.CreateCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
    }
}