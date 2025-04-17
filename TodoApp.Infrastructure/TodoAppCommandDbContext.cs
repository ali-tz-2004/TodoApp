using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Entities.Todo;
using TodoApp.Infrastructure.Mappers.Auth;
using TodoApp.Infrastructure.Mappers.Todo;

namespace TodoApp.Infrastructure;

public class TodoAppCommandDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    public TodoAppCommandDbContext(DbContextOptions<TodoAppCommandDbContext> options) 
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryMapper).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoMapper).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserMapper).Assembly);
        
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<TodoItem>().HasQueryFilter(x => !x.IsDelete);
    }
}