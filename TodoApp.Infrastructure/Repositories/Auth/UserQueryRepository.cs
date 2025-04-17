using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Infrastructure.Repositories.Auth;

public class UserQueryRepository(TodoAppQueryDbContext dbContext) : IUserQueryRepository
{
    public async Task<User> Login(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username, cancellationToken);
        if(user == null) throw new NotFoundException("Invalid username or password");
        
        return user;
    }

    public async Task<bool> ExistsUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }
}