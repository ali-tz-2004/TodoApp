using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Infrastructure.Repositories.Todo;

public class TodoQueryRepository(TodoAppQueryDbContext dbContext) : ITodoQueryRepository
{
    public async Task<TodoItem> GetById(int id, CancellationToken cancellationToken)
    {
        var todo = await dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (todo == null) throw new NotFoundException("todo not found");
        return todo;
    }

    public async Task<List<TodoItem>> GetAll(Guid userId, CancellationToken cancellationToken)
    {
        var todoList = await dbContext.TodoItems.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        return todoList;
    }

    public async Task<bool> ExistsCategoryAsync(int categoryId, Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Categories.AnyAsync(x => x.Id == categoryId && x.UserId == userId, cancellationToken);
    }
}