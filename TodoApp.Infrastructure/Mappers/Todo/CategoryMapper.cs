using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Infrastructure.Mappers.Todo;

public class CategoryMapper : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(TodoAppQueryDbContext.Categories), schema: TodoAppDbSchema.Todo);
        builder.Property(x=> x.Name).IsRequired().HasMaxLength(100);
        
        builder
            .HasOne(x => x.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}