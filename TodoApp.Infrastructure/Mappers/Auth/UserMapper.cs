using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Infrastructure.Mappers.Auth;

public class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(TodoAppQueryDbContext.Users), schema: TodoAppDbSchema.User);
        builder.Property(x=> x.UserName).IsRequired().HasMaxLength(120);
        builder.Property(x=> x.Email).HasMaxLength(120);
        builder.Property(x=> x.Password).IsRequired().HasMaxLength(120);
        builder.Property(x=>x.PasswordSalt).IsRequired().HasMaxLength(120);
    }
}