using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepositoryMock = new();
    private readonly Mock<ITodoQueryRepository> _todoQueryRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly int _id = 1;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly string _name = "Test";

    private  DeleteCategoryRequest _categoryRequest => new()
    {
        Id = _id,
        UserId = _userId,
    };
    
    [Fact]
    public async Task Should_Delete_Category()
    {
        var category = new Category(_name, _userId);
        
        _todoQueryRepositoryMock.Setup(x => x.GetByIdCategory(_id, _userId, CancellationToken.None))
            .ReturnsAsync(category);

        var handler = new DeleteCategoryCommandHandler(_todoCommandRepositoryMock.Object,
            _todoQueryRepositoryMock.Object, _unitOfWorkMock.Object);
        
        await handler.Handle(_categoryRequest, CancellationToken.None);
        
        _todoCommandRepositoryMock.Verify(x=>x.DeleteCategory(category), Times.Once);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Delete_Category_If_Not_Found()
    {
        var category = new Category(_name, _userId);
        
        _todoQueryRepositoryMock.Setup(x => x.GetByIdCategory(_id, _userId, CancellationToken.None))
            .ThrowsAsync(new NotFoundException(nameof(Category)));
        
        var handler = new DeleteCategoryCommandHandler(_todoCommandRepositoryMock.Object,
            _todoQueryRepositoryMock.Object, _unitOfWorkMock.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(_categoryRequest, CancellationToken.None));
        
        _todoCommandRepositoryMock.Verify(x=>x.DeleteCategory(category), Times.Never);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}