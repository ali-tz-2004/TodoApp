using MediatR;
using TodoApp.Common;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Application.Auth.Commands.RegisterCommand;

public class RegisterCommandHandler(
    IEncryptionUtility encryptionUtility,
    IUserCommandRepository userCommandRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var salt = encryptionUtility.GetNewSalt();
        var hashPassword = encryptionUtility.GetSHA256(command.Password, salt);
        
        var user = User.CreateUser(command.UserName, command.Email, hashPassword, salt);
        
        userCommandRepository.CreateUser(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}