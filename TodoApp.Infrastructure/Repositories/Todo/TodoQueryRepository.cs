using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Todo;
using TodoApp.Core.Interfaces.Repositories.Todo;

namespace TodoApp.Infrastructure.Repositories.Todo;

public class TodoQueryRepository(TodoAppQueryDbContext dbContext) : ITodoQueryRepository
{
    public async Task<TodoItem> GetById(int id, Guid userId, CancellationToken cancellationToken)
    {
        var todo = await dbContext.TodoItems.Include(x => x.User).Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
        if (todo == null) throw new NotFoundException("todo not found");
        return todo;
    }

    public async Task<List<TodoItem>> GetAll(bool isCompleted, Guid userId, CancellationToken cancellationToken)
    {
        var todoList = await dbContext.TodoItems.Where(x => x.UserId == userId && x.IsCompleted == isCompleted).ToListAsync(cancellationToken);
        return todoList;
    }

    public async Task<bool> ExistsCategoryAsync(int categoryId, Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Categories.AnyAsync(x => x.Id == categoryId && x.UserId == userId, cancellationToken);
    }

    public async Task<Category> GetByIdCategory(int id, Guid userId, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);
        if (category == null) throw new NotFoundException("todo not found");
        return category;
    }

    public async Task<List<Category>> GetAllCategory(Guid userId, CancellationToken cancellationToken)
    {
        var todoList = await dbContext.Categories.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        return todoList;
    }
}