using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    
    [Fact]
    public async Task Handle_Should_CreateCategory_When_Valid()
    {
        var handler = new CreateCategoryCommandHandler(_todoCommandRepositoryMock.Object, _unitOfWorkMock.Object);

        var create = new CreateCategoryRequest()
        {
            Name = "Name",
            UserId = Guid.NewGuid()
        };
        
        await handler.Handle(create, CancellationToken.None);
        
        _todoCommandRepositoryMock.Verify(x=>x.CreateCategory(It.IsAny<Category>()), Times.Once());
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
}