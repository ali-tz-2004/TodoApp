using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Application.Todo.Queries.GetAllTodoQuery;

public class GetAllTodoResponse
{
    public GetAllTodoResponse(TodoItem todoItem)
    {
        Id = todoItem.Id;
        Title = todoItem.Title;
        Description = todoItem.Description;
        DueDate = todoItem.DueDate;
        DuePersianDate = todoItem.DueDate.GetUtcNow();
        IsCompleted = todoItem.IsCompleted;
        CategoryId = todoItem.CategoryId;
        UserId = todoItem.UserId;
    }

    public GetAllTodoResponse()
    {
        
    }
    
    public int Id { get; set; } 
    public string Title { get; set; } 
    public string? Description { get; set; } 
    public DateOnly? DueDate { get; set; }
    public string? DuePersianDate { get; set; }
    public bool IsCompleted { get; set; }
    public int CategoryId { get; set; }
    public Guid UserId { get; set; } 
    
}