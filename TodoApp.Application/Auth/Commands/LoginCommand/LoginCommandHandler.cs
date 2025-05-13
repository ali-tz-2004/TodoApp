using MediatR;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Application.Auth.Commands.LoginCommand;

public class LoginCommandHandler(IUserQueryRepository userQueryRepository, IEncryptionUtility encryptionUtility) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userQueryRepository.Login(command.UserName, command.Password, cancellationToken);
        
        var hashPassword = encryptionUtility.GetSHA256(command.Password, user.PasswordSalt);
        if (user.Password != hashPassword) throw new NotFoundException("Invalid username or password");
        
        var token = encryptionUtility.GetNewToken(user.Id);

        var response = new LoginResponse
        {
            UserName = command.UserName,
            Token = token
        };
        
        return response;
    }
}