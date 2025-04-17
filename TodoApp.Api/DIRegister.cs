using Microsoft.EntityFrameworkCore;
using TodoApp.Common;
using TodoApp.Common.Utilities;
using TodoApp.Core.Interfaces.Repositories.Auth;
using TodoApp.Core.Interfaces.Repositories.Todo;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Repositories.Auth;
using TodoApp.Infrastructure.Repositories.Todo;
using TodoApp.Infrastructure.Utilities;

namespace TodoApp.Api;

public static class DIRegister
{
    public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoAppCommandDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CommandDbConnection")));
        services.AddDbContext<TodoAppQueryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QueryDbConnection")));

        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<ITodoCommandRepository, TodoCommandRepository>();
        services.AddScoped<ITodoQueryRepository, TodoQueryRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEncryptionUtility, EncryptionUtility>();
    }
}