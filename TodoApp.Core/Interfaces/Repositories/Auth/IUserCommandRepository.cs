using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Interfaces.Repositories.Auth;

public interface IUserCommandRepository
{
    void CreateUser(User user);
}