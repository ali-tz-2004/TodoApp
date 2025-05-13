using MediatR;

namespace TodoApp.Application.Auth.Commands.RegisterCommand;

public class RegisterCommand : IRequest
{
    public required string UserName { get; set; }
    public required string Email { get; set; } 
    public required string Password { get; set; }
}