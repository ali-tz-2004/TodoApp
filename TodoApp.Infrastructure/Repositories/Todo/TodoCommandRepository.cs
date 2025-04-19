using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Infrastructure.Repositories.Todo;

public class TodoCommandRepository(TodoAppCommandDbContext dbContext) : ITodoCommandRepository
{
    public void CreateTodo(TodoItem todoItem)
    {
        todoItem.CreateBase(todoItem.Id);
        dbContext.TodoItems.Add(todoItem);
    }

    public void UpdateTodo(TodoItem todoItem)
    {
        todoItem.UpdateBase();
        dbContext.TodoItems.Update(todoItem);
    }

    public void ChangeStatus(TodoItem todoItem)
    {
        dbContext.Entry(todoItem).Property(x => x.IsCompleted).IsModified = true;
    }

    public void DeleteTodo(TodoItem todoItem)
    {
        todoItem.DeleteBase();
        dbContext.Update(todoItem);
    }
}