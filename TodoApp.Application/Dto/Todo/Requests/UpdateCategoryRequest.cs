using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Dto.Todo.Requests;

public class UpdateCategoryRequest : IRequest, IHaveUserId
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}