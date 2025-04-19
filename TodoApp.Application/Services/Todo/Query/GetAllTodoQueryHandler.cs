using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.Utilities;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Query;

public class GetAllTodoQueryHandler(ITodoQueryRepository todoQueryRepository) : IRequestHandler<GetAllTodoRequest, PaginationResponse<GetAllTodoResponse>>
{
    public async Task<PaginationResponse<GetAllTodoResponse>> Handle(GetAllTodoRequest request, CancellationToken cancellationToken)
    {
        var todoList = await todoQueryRepository.GetAll(request.UserId, cancellationToken);

        var result = todoList.Select(x => new GetAllTodoResponse(x)).ToList();
        
        return result.Paginate(request.Page, request.PageSize);
    }
}