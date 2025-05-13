using MediatR;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.Interfaces;

namespace TodoApp.Application.Todo.Queries.GetAllTodoQuery;

public class GetAllTodoQuery : PaginationRequest, IRequest<PaginationResponse<GetAllTodoResponse>>, IHaveUserId
{
    public bool IsCompleted { get; set; } = false;
    public Guid UserId { get; set; }
}