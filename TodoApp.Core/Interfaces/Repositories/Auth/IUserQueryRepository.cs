using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Interfaces.Repositories.Auth;

public interface IUserQueryRepository
{
    Task<User> GetByUserName(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserName(string username, CancellationToken cancellationToken);
    Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken);
}