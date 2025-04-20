using System.Text.Json.Serialization;
using MediatR;

namespace TodoApp.Application.Dto.Todo.Requests;

public class CreateCategoryRequest : IRequest
{
    public string Name { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}