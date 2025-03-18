using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GreenSpace.Application.Services;

public class JWTTokenGenerator : IJWTTokenGenerator
{
    private readonly AppSettings appSettings;
    public JWTTokenGenerator(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }
    public string GenerateToken(User user, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.JWTOptions.Secret);
        var claimsList = new List<Claim>()
            {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.HomePhone, user.Phone!),
            new(ClaimTypes.Role, role)
            };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = appSettings.JWTOptions.Audience,
            Issuer = appSettings.JWTOptions.Issuer,
            Subject = new ClaimsIdentity(claimsList),
            Expires = DateTime.UtcNow.AddDays(1000),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
