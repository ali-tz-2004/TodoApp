using MediatR;
using TodoApp.Common.DtoHandler;
using TodoApp.Common.Utilities;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Queries.GetAllTodoQuery;

public class GetAllTodoQueryHandler(ITodoQueryRepository todoQueryRepository) : IRequestHandler<GetAllTodoQuery, PaginationResponse<GetAllTodoResponse>>
{
    public async Task<PaginationResponse<GetAllTodoResponse>> Handle(GetAllTodoQuery query, CancellationToken cancellationToken)
    {
        var todoList = await todoQueryRepository.GetAll(query.IsCompleted, query.UserId, cancellationToken);

        var result = todoList.Select(x => new GetAllTodoResponse(x)).ToList();
        
        return result.Paginate(query.Page, query.PageSize);
    }
}