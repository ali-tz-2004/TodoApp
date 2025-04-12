using MediatR;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Application.Dto.Auth.Response;
using TodoApp.Common.Utilities;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Application.Services.Auth.Query;

public class LoginQueryHandler(IUserQueryRepository userQueryRepository, IEncryptionUtility encryptionUtility) : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userQueryRepository.Login(request.UserName, request.Password);
        
        var hashPassword = encryptionUtility.GetSHA256(request.Password, user.PasswordSalt);
        if (user.Password != hashPassword) throw new Exception("invalid password");
        
        var token = encryptionUtility.GetNewToken(user.Id);

        var response = new LoginResponse
        {
            UserName = request.UserName,
            Token = token
        };
        
        return response;
    }
}