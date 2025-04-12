namespace TodoApp.Common.Utilities;

public interface IEncryptionUtility
{
    string GetSHA256(string password, string salt);
    string GetNewSalt();
    string GetNewToken(Guid userId);
}