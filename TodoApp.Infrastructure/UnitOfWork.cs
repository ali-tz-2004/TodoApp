using TodoApp.Common;

namespace TodoApp.Infrastructure;

public class UnitOfWork(TodoAppCommandDbContext appCommandDbContext) : IUnitOfWork, IAsyncDisposable
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await appCommandDbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await appCommandDbContext.DisposeAsync();
    }
}