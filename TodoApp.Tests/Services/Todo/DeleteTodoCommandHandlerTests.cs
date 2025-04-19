using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
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

    private DeleteTodoRequest _request() => new()
    {
        Id = _id,
        UserId = _userId,
    };

    [Fact]
    public async Task Handle_Should_Delete_Todo_When_Valid()
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
    
    

}