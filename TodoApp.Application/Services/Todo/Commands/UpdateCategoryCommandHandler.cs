using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class UpdateCategoryCommandHandler(ITodoQueryRepository todoQueryRepository, 
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateCategoryRequest>
{
    public async Task Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await todoQueryRepository.GetByIdCategory(request.Id, request.UserId, CancellationToken.None);
        category.UpdateCategory(request.Name);
        
        todoCommandRepository.UpdateCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}