using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Infrastructure.Mappers.Todo;

public class TodoMapper: IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable(nameof(TodoAppQueryDbContext.TodoItems), schema: TodoAppDbSchema.Todo);
        builder.Property(x=> x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x=> x.Description).HasMaxLength(1000);
        
        builder
            .HasOne(t => t.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}