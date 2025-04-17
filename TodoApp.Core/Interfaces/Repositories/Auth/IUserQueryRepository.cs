using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Interfaces.Repositories.Auth;

public interface IUserQueryRepository
{
    Task<User> Login(string username, string password, CancellationToken cancellationToken = default);
    Task<bool> ExistsUserAsync(Guid userId, CancellationToken cancellationToken);
}