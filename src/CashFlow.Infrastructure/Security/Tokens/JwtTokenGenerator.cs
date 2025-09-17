using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Infrastructure.Security.Tokens
{
  public class JwtTokenGenerator(uint _expirationTimeMinutes, string _signingKey) : IAcessTokenGenerator
  {
    public string Generate(User user)
    {
      var claims = new List<Claim>()
      {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Sid, user.UserIdIdentifier.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
      };

      var tokenDescription = new SecurityTokenDescriptor
      {
        Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
        SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature), 
        Subject = new ClaimsIdentity(claims)
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var securityToken = tokenHandler.CreateToken(tokenDescription);

      return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey GetSecurityKey()
    {
      var key = Encoding.UTF8.GetBytes(_signingKey);

      return new SymmetricSecurityKey(key);
    }
  }
}
