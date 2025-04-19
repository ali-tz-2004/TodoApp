using MediatR;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Dto.Todo.Requests;

public class GetTodoRequest : IRequest<GetTodoResponse>, IHaveUserId
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}