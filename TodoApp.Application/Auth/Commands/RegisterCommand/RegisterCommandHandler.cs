using MediatR;
using TodoApp.Common;
using TodoApp.Common.Exceptions;
using TodoApp.Common.Utilities;
using TodoApp.Core.Entities.Auth;
using TodoApp.Core.Interfaces.Repositories.Auth;

namespace TodoApp.Application.Auth.Commands.RegisterCommand;

public class RegisterCommandHandler(
    IEncryptionUtility encryptionUtility,
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var isExistsByUserName = await userQueryRepository.ExistsByUserName(command.UserName, cancellationToken);
        if (isExistsByUserName) throw new ConflictException("UserName already exists");
        
        var iExistsByEmail = await userQueryRepository.ExistsByEmail(command.Email, cancellationToken);
        if (iExistsByEmail) throw new ConflictException("Email already exists");
        
        var salt = encryptionUtility.GetNewSalt();
        var hashPassword = encryptionUtility.GetSHA256(command.Password, salt);
        
        var user = User.CreateUser(command.UserName, command.Email, hashPassword, salt);
        
        userCommandRepository.CreateUser(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}