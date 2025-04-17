using TodoApp.Core.Entities.Todo;

namespace TodoApp.Core.Interfaces.Repositories.Todo;

public interface ITodoQueryRepository
{
    Task<TodoItem> GetById(int id, CancellationToken cancellationToken);
    Task<List<TodoItem>> GetAll(Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistsCategoryAsync(int categoryId, Guid userId, CancellationToken cancellationToken);
}