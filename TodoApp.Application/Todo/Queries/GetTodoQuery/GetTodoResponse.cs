using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Application.Todo.Queries.GetTodoQuery;

public class GetTodoResponse(TodoItem todoItem)
{
    public int Id { get; set; } = todoItem.Id;
    public string Title { get; set; } = todoItem.Title;
    public string? Description { get; set; } = todoItem.Description;
    public DateOnly? DueDate { get; set; } = todoItem.DueDate;
    public string? DuePersianDate { get; set; } = todoItem.DueDate.GetUtcNow();
    public bool IsCompleted { get; set; } = todoItem.IsCompleted;
    public int CategoryId { get; set; } = todoItem.CategoryId;
    public string Category { get; set; } = todoItem.Category.Name;
    public Guid UserId { get; set; } = todoItem.UserId;
    public string Username { get; set; } = todoItem.User.UserName;
}
