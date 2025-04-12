using TodoApp.Core.Entities.Todo;

namespace TodoApp.Core.Interfaces.Repositories.Todo;

public interface ITodoQueryRepository
{
    TodoItem GetById(int id);
    List<TodoItem> GetAll();
}