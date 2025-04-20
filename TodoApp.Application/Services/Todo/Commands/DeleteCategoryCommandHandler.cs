using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Commands;

public class DeleteCategoryCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    ITodoQueryRepository todoQueryRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCategoryRequest>
{
    public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await todoQueryRepository.GetByIdCategory(request.Id, request.UserId, CancellationToken.None);
        todoCommandRepository.DeleteCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}