using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure;

namespace TodoApp.Api;

public static class DIRegister
{
    public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // context
        services.AddDbContext<TodoAppCommandDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CommandDbConnection")));
        services.AddDbContext<TodoAppQueryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QueryDbConnection")));
        
    }
}