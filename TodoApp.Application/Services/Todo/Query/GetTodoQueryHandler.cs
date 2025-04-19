using MediatR;
using TodoApp.Application.Dto.Todo.Requests;
using TodoApp.Application.Dto.Todo.Response;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Application.Services.Todo.Query;

public class GetTodoQueryHandler(ITodoQueryRepository todoQueryRepository) : IRequestHandler<GetTodoRequest, GetTodoResponse>
{
    public async Task<GetTodoResponse> Handle(GetTodoRequest request, CancellationToken cancellationToken)
    {
        var todoItem = await todoQueryRepository.GetById(request.Id, request.UserId, cancellationToken);
        var result = new GetTodoResponse(todoItem);
        
        return result;
    }
}