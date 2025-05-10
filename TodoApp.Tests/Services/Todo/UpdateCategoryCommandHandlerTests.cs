using Moq;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Services.Todo.Commands;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Tests.Services.Todo;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<ITodoQueryRepository> _todoQueryRepository = new();
    private readonly Mock<ITodoCommandRepository> _todoCommandRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    
    private int _id = 1;
    private Guid _userId = Guid.NewGuid();
    
    private UpdateCategoryRequest _request => new()
    {
        Id = _id,
        UserId = _userId,
        Name = "NewName"
    };
    
    [Fact]
    public async Task when_update_category_successfully()
    {
        var existingCategory  = new Category("OldName", _userId); 
        
        _todoQueryRepository.Setup(x=>x.GetByIdCategory(_id, _userId, CancellationToken.None))
            .ReturnsAsync(existingCategory);

        var handler = new UpdateCategoryCommandHandler(_todoQueryRepository.Object, _todoCommandRepository.Object, _unitOfWork.Object);


        
        await handler.Handle(_request, CancellationToken.None);
        
        _todoCommandRepository.Verify(x=>x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        _unitOfWork.Verify(x=>x.SaveChangesAsync(CancellationToken.None));
        
        Assert.Equal("NewName", existingCategory.Name); 
    }
    
    [Fact]
    public async Task when_category_id_or_user_id_not_found()
    {
        _todoQueryRepository.Setup(x=>x.GetByIdCategory(_id, _userId, CancellationToken.None))
            .ThrowsAsync(new NotFoundException(nameof(Category)));

        var handler = new UpdateCategoryCommandHandler(_todoQueryRepository.Object, _todoCommandRepository.Object, _unitOfWork.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(_request, CancellationToken.None));
        
        _todoCommandRepository.Verify(x=>x.UpdateCategory(It.IsAny<Category>()), Times.Never());
        _unitOfWork.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}