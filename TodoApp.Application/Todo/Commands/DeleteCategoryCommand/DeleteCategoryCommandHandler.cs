using MediatR;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.DeleteCategoryCommand;

public class DeleteCategoryCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    ITodoQueryRepository todoQueryRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await todoQueryRepository.GetByIdCategory(command.Id, command.UserId, CancellationToken.None);
        todoCommandRepository.DeleteCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}