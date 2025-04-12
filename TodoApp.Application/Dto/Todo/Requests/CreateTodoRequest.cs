using MediatR;

namespace TodoApp.Application.Dto.Todo.Requests;

public class CreateTodoRequest : IRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public int CategoryId { get; set; }
    public Guid UserId { get; set; }
}