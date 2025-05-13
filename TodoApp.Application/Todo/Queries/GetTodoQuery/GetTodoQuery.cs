using MediatR;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Queries.GetTodoQuery;

public class GetTodoQuery : IRequest<GetTodoResponse>, IHaveUserId
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}