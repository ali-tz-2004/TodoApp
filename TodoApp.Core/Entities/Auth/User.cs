using System.Text.RegularExpressions;
using TodoApp.Common.Exceptions;
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
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (string.IsNullOrWhiteSpace(userName))
            throw new NotValidException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(email))
            throw new NotValidException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(password))
            throw new NotValidException("Password cannot be empty");

        if (string.IsNullOrWhiteSpace(passwordSalt))
            throw new NotValidException("Password salt cannot be empty");

        if (!Regex.IsMatch(email, pattern))
            throw new NotValidException("Email is not valid");
        
        return new User(userName, email, password, passwordSalt);
    }
}