using TodoApp.Core.Entities.Todo;

namespace TodoApp.Core.Interfaces.Repositories.Todo;

public interface ITodoCommandRepository
{
    void CreateTodo(TodoItem todoItem);
    void UpdateTodo(TodoItem todoItem);

}