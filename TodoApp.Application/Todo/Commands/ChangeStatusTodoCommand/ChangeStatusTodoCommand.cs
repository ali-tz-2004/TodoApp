using System.Text.Json.Serialization;
using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Commands.ChangeStatusTodoCommand;

public class ChangeStatusTodoCommand : IRequest, IHaveUserId
{
    public int Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}