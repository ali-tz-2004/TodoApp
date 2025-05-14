using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
}
