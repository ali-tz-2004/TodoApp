using TodoApp.Core.Base;
using TodoApp.Core.Entities.Todo;

namespace TodoApp.Core.Entities.Auth;

public class User : BaseEntity<Guid>
{
    public string UserName { get; private set; }
    public string Email { get; private set; } 
    public string Password { get; private set; }
    public string PasswordSalt { get; private set; }
    
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    
    private User() { }
    
    public User(string userName, string email, string password, string passwordSalt)
    {
        UserName = userName;
        Email = email;
        Password = password;
        PasswordSalt = passwordSalt;
    }

    public static User CreateUser(string userName, string email, string password, string passwordSalt)
    {
        return new User(userName, email, password, passwordSalt);
    }
    
    public static User CreateUser(Guid id, string userName, string email, string password, string passwordSalt)
    {
        var user = new User(userName, email, password, passwordSalt);
        user.Id = id;
        return user;
    }
}