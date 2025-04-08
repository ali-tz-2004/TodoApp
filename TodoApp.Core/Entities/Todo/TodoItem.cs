using TodoApp.Core.Base;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Entities.Todo;

public class TodoItem : BaseEntity<int>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private TodoItem()
    {
        
    }

    public TodoItem(string title, string? description, DateTime? dueDate, bool isCompleted, int categoryId, Guid userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = isCompleted;
        CategoryId = categoryId;
        UserId = userId;
    }
    
}