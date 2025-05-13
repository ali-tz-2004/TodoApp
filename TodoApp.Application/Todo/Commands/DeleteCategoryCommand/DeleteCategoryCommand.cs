using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Commands.DeleteCategoryCommand;

public class DeleteCategoryCommand : IHaveUserId, IRequest
{
    public int Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}