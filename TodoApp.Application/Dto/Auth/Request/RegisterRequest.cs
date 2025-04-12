using MediatR;

namespace TodoApp.Application.Dto.Auth.Request;

public class RegisterRequest : IRequest
{
    public required string UserName { get; set; }
    public required string Email { get; set; } 
    public required string Password { get; set; }
}