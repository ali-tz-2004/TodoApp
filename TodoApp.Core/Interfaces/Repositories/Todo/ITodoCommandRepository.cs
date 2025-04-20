using TodoApp.Core.Entities.Todo;

namespace TodoApp.Core.Interfaces.Repositories.Todo;

public interface ITodoCommandRepository
{
    void CreateTodo(TodoItem todoItem);
    void UpdateTodo(TodoItem todoItem);
    void ChangeStatus(TodoItem todoItem);
    void DeleteTodo(TodoItem todoItem);
    
    void CreateCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);
}