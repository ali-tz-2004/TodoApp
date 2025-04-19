using MediatR;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.Interfaces;


namespace TodoApp.Application.Dto.Todo.Requests;

public class GetAllTodoRequest : PaginationRequest, IRequest<PaginationResponse<GetAllTodoResponse>>, IHaveUserId
{
    public bool IsCompleted { get; set; } = false;
    public Guid UserId { get; set; }
}