using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Api;
using TodoApp.Infrastructure;

namespace TodoApp.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        
        builder.ConfigureServices(services =>
        {
            RemoveDbContext<TodoAppCommandDbContext>(services);
            RemoveDbContext<TodoAppQueryDbContext>(services);

            services.AddDbContext<TodoAppCommandDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            
            services.AddDbContext<TodoAppQueryDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            
            var commandDb = scope.ServiceProvider.GetRequiredService<TodoAppCommandDbContext>();
            commandDb.Database.EnsureCreated();
            
            var queryDb = scope.ServiceProvider.GetRequiredService<TodoAppQueryDbContext>();
            queryDb.Database.EnsureCreated();
            
        });
    }

    private static void RemoveDbContext<TDbContext>(IServiceCollection services) where TDbContext : DbContext
    {
        var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<TDbContext>));
        
        if (dbContext == null) return;
        
        services.Remove(dbContext);
    }
}
