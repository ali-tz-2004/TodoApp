using TodoApp.Core.Base;

namespace TodoApp.Core.Entities.Todo;

public class Category : BaseEntity<int>
{
    public string Name { get; private set; }

    public ICollection<TodoItem> Items { get; private set; } = [];

    private Category()
    {
        
    }

    public Category(string name)
    {
        Name = name;
    }
}