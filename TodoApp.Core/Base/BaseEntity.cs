namespace TodoApp.Core.Base;

public class BaseEntity<TKey>
{
    public TKey Id { get; protected set; } 
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDelete { get; private set; }
    
    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
        IsDelete = false;
    }
    
    public void CreateBase(TKey id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        IsDelete = false;
    }
    
    public void UpdateBase()
    {
        UpdatedAt =  DateTime.Now;
    }

    public void DeleteBase()
    {
        IsDelete = true;
    }
}