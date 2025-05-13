using MediatR;
using TodoApp.Common;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommandHandler(ITodoQueryRepository todoQueryRepository, 
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await todoQueryRepository.GetByIdCategory(command.Id, command.UserId, CancellationToken.None);
        category.UpdateCategory(command.Name);
        
        todoCommandRepository.UpdateCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}