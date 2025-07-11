using Moq;
using TodoApp.Application.Todo.Commands.CreateCategoryCommand;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    
    [Fact]
    public async Task when_create_category_valid()
    {
        var handler = new CreateCategoryCommandHandler(_todoCommandRepositoryMock.Object, _unitOfWorkMock.Object);

        var request = new CreateCategoryCommand()
        {
            Name = "Name",
            UserId = Guid.NewGuid()
        };
        
        await handler.Handle(request, CancellationToken.None);
        
        _todoCommandRepositoryMock.Verify(x=>x.CreateCategory(It.IsAny<Category>()), Times.Once());
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task when_category_name_is_null_or_empty_string(string name)
    {
        var handler = new CreateCategoryCommandHandler(_todoCommandRepositoryMock.Object, _unitOfWorkMock.Object);

        var request = new CreateCategoryCommand()
        {
            Name = name,
            UserId = Guid.NewGuid()
        };
        
        var ex = await Assert.ThrowsAsync<NotValidException>(() => handler.Handle(request, CancellationToken.None));
        
        Assert.Contains("name cannot be empty.", ex.Message);
        
        _todoCommandRepositoryMock.Verify(x=>x.CreateCategory(It.IsAny<Category>()), Times.Never);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}