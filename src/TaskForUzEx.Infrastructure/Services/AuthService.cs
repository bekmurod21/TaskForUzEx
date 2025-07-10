using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskForUzEx.Application.Abstractions;
using TaskForUzEx.Domain.Entities;

namespace TaskForUzEx.Infrastructure.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public string GenerateJwtToken(User user)
    {
        var tokenHendler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDisctiptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user?.Role.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
            }),
            Audience = configuration["JWT:Audience"],
            Issuer = configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(int.Parse(configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHendler.CreateToken(tokenDisctiptor);
        return tokenHendler.WriteToken(token);
    }
}