using MediatR;
using TodoApp.Application.Dto.Auth.Request;
using TodoApp.Common;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Application.Services.Auth.Command;

public class RegisterCommandHandler(
    IEncryptionUtility encryptionUtility,
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterRequest>
{
    public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var salt = encryptionUtility.GetNewSalt();
        var hashPassword = encryptionUtility.GetSHA256(request.Password, salt);
        
        var user = User.CreateUser(request.UserName, request.Email, hashPassword, salt);
        
        userCommandRepository.CreateUser(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}