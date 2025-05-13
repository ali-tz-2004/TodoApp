using MediatR;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Todo.Queries.GetTodoQuery;

public class GetTodoQueryHandler(ITodoQueryRepository todoQueryRepository) : IRequestHandler<GetTodoQuery, GetTodoResponse>
{
    public async Task<GetTodoResponse> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todoItem = await todoQueryRepository.GetById(query.Id, query.UserId, cancellationToken);
        var result = new GetTodoResponse(todoItem);
        
        return result;
    }
}