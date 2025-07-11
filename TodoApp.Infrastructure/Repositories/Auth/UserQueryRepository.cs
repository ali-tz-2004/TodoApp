using Microsoft.EntityFrameworkCore;
using TodoApp.Common.Exceptions;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Infrastructure.Repositories.Auth;

public class UserQueryRepository(TodoAppQueryDbContext dbContext) : IUserQueryRepository
{
    public async Task<User> GetByUserName(string username, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username, cancellationToken);
        if(user == null) throw new NotFoundException("Invalid username or password");
        
        return user;
    }

    public async Task<bool> ExistsByUserName(string username, CancellationToken cancellationToken) => 
        await dbContext.Users.AnyAsync(u => u.UserName == username, cancellationToken);

    public async Task<bool> ExistsByEmail(string email, CancellationToken cancellationToken) =>
        await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
}