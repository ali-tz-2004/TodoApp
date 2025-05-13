using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Commands.CreateCategoryCommand;

public class CreateCategoryCommand : IRequest, IHaveUserId
{
    public string Name { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}