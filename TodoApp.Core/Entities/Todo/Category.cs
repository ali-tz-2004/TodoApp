using TodoApp.Common.Exceptions;
using TodoApp.Core.Base;
using TodoApp.Core.Entities.Auth;

namespace TodoApp.Core.Entities.Todo;

public class Category : BaseEntity<int>
{
    public string Name { get; private set; }
    public Guid UserId { get; private set; }
    
    public User User { get; private  set; } = null!;
    public ICollection<TodoItem> Items { get; private set; } = [];

    private Category()
    {
        
    }

    public Category(string name, Guid userId)
    {
        Name = name;
        UserId = userId;
    }
    
    public static Category CreateCategory(string name, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new NotValidException("name cannot be empty.");
        
        var category = new Category(name, userId);
        return category;
    }
    
    public void UpdateCategory(string name)
    {
        Name = name;
    }
}