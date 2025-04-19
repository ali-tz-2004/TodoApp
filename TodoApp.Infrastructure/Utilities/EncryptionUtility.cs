using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Common.Utilities;
using TodoApp.Infrastructure.Models;

namespace TodoApp.Infrastructure.Utilities;

public class EncryptionUtility : IEncryptionUtility
{
    private readonly Configs configs;
    public EncryptionUtility(IOptions<Configs> optionsAccessor)
    {
        configs = optionsAccessor.Value;
    }
    public string GetSHA256(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            var hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return hash;
        }
    }

    public string GetNewSalt()
    {
        return Guid.NewGuid().ToString();
    }

    public string GetNewToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configs.TokenKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(configs.TokenTimeout),
            Issuer = configs.TokenIssuer,
            Audience = configs.TokenAudience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}