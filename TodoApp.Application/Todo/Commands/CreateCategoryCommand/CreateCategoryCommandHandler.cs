using MediatR;
using TodoApp.Common;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Commands.CreateCategoryCommand;

public class CreateCategoryCommandHandler(
    ITodoCommandRepository todoCommandRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateCategoryCommand>
{
    public async Task Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.CreateCategory(command.Name, command.UserId);
        
        todoCommandRepository.CreateCategory(category);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
    }
}