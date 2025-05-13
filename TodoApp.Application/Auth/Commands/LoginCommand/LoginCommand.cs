using MediatR;

namespace TodoApp.Application.Auth.Commands.LoginCommand;

public class LoginCommand : IRequest<LoginResponse>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}