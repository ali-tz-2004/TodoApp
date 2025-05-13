using Moq;
using TodoApp.Application.Todo.Commands;
using TodoApp.Application.Todo.Commands.DeleteTodoCommand;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class DeleteTodoCommandHandlerTests
{
    private readonly Mock<ITodoQueryRepository> _todoQueryRepositoryMock = new();
    private readonly Mock<ITodoCommandRepository> _todoCommandRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    
    private readonly Guid _userId = Guid.NewGuid();
    private readonly int _id = 1;
    private readonly int _categoryId = 1;

    private DeleteTodoCommand _request() => new()
    {
        Id = _id,
        UserId = _userId,
    };

    [Fact]
    public async Task when_deleted_todo_successfully()
    {
        var request = _request();
        
        var fakeTodoItem = new TodoItem("Old Title", "Old Desc", DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            _categoryId, _userId);
        
        _todoQueryRepositoryMock.Setup(x => x.GetById(_id, _userId, CancellationToken.None))
            .ReturnsAsync(fakeTodoItem);

        var handler = new DeleteTodoCommandHandler(_todoQueryRepositoryMock.Object, _todoCommandRepository.Object,
            _unitOfWork.Object);
        
        await handler.Handle(request, CancellationToken.None);
        
        _todoCommandRepository.Verify(x=>x.DeleteTodo(fakeTodoItem), Times.Once);
        _unitOfWork.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
    
    
    [Fact]
    public async Task when_todo_id_or_user_id_not_found()
    {
        var request = _request();
        
        var fakeTodoItem = new TodoItem("Old Title", "Old Desc", DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            _categoryId, _userId);

        _todoQueryRepositoryMock.Setup(x => x.GetById(_id, _userId, CancellationToken.None))
            .ThrowsAsync(new NotFoundException(nameof(TodoItem)));

        var handler = new DeleteTodoCommandHandler(_todoQueryRepositoryMock.Object, _todoCommandRepository.Object,
            _unitOfWork.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
        
        _todoCommandRepository.Verify(x=>x.DeleteTodo(fakeTodoItem), Times.Never);
        _unitOfWork.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}