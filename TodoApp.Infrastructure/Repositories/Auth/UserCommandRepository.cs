using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Infrastructure.Repositories.Auth;

public class UserCommandRepository(TodoAppCommandDbContext dbContext) : IUserCommandRepository
{
    public void CreateUser(User user)
    {
        user.CreateBase(user.Id);
        dbContext.Users.Add(user);
    }
}

