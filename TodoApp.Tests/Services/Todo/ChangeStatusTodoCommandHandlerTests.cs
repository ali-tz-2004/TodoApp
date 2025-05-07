using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class ChangeStatusTodoCommandHandlerTests
{
    private readonly Mock<ITodoCommandRepository> _todoCommandRepositoryMock = new();
    private readonly Mock<ITodoQueryRepository> _todoQueryRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly int _requestId = 1;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly int _categoryId = 1;

    private ChangeStatusTodoRequest _request() => new()
    {
        Id = _requestId,
        UserId = _userId,
    };

    [Fact]
    public async Task Handle_ValidData_ShouldChangeStatus()
    {
        var request = _request();
        
        var fakeTodoItem = new TodoItem("Old Title", "Old Desc", DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            _categoryId, _userId);
        
        Assert.False(fakeTodoItem.IsCompleted);
        
        _todoQueryRepositoryMock.Setup(x => x.GetById(_requestId, _userId, CancellationToken.None))
            .ReturnsAsync(fakeTodoItem);

        var handler = new ChangeStatusTodoCommandHandler(_todoCommandRepositoryMock.Object,
            _todoQueryRepositoryMock.Object, _unitOfWorkMock.Object);

        await handler.Handle(request, CancellationToken.None);
        
        _todoCommandRepositoryMock.Verify(x=>x.ChangeStatus(fakeTodoItem), Times.Once);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once);
        
        Assert.True(fakeTodoItem.IsCompleted);
    }
    
    [Fact]
    public async Task Handle_WhenUserIdOrTodoNotFound_ShouldReturnNotFound()
    {
        _todoQueryRepositoryMock.Setup(x => x.GetById(_requestId, _userId, CancellationToken.None))
            .ThrowsAsync(new NotFoundException("todo not found"));
        
        var request = _request();
        
        var handler = new ChangeStatusTodoCommandHandler(_todoCommandRepositoryMock.Object,
            _todoQueryRepositoryMock.Object, _unitOfWorkMock.Object);
        
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        
        _todoCommandRepositoryMock.Verify(x=>x.ChangeStatus(It.IsAny<TodoItem>()), Times.Never);
        _unitOfWorkMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Never);
        
        Assert.Equal("todo not found", ex.Message);
    }
}