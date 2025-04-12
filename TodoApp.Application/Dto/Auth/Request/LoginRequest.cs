using MediatR;
using TodoApp.Application.Dto.Auth.Response;

namespace TodoApp.Application.Dto.Auth.Request;

public class LoginRequest : IRequest<LoginResponse>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}