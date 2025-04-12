using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Infrastructure.Repositories.Auth;

public class UserQueryRepository(TodoAppQueryDbContext dbContext) : IUserQueryRepository
{
    public async Task<User> Login(string username, string password)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
        if(user == null) throw new Exception("Invalid username or password");
        
        return user;
    }
}