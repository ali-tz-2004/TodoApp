using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommand : IRequest, IHaveUserId
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}