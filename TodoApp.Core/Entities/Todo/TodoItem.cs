using TodoApp.Core.Base;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Entities.Todo;

public class TodoItem : BaseEntity<int>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private TodoItem()
    {
        
    }

    public TodoItem(string title, string? description, DateOnly? dueDate, int categoryId, Guid userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = false;
        CategoryId = categoryId;
        UserId = userId;
    }

    public static TodoItem CreateTodo(string title, string? description, DateOnly? dueDate, int categoryId, Guid userId)
    {
        var user = new TodoItem(title, description, dueDate, categoryId, userId);
        return user;
    }
    
    public void UpdateTodo(string title, string? description, DateOnly? dueDate, int categoryId, Guid userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        CategoryId = categoryId;
        UserId = userId;
    }
}