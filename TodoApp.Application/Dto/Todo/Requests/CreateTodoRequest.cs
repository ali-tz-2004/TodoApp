using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Dto.Todo.Requests;

public class CreateTodoRequest : IRequest, IHaveUserId
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateOnly? DueDate { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}