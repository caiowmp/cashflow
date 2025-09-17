using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Services.LoggedUser
{
  public class LoggedUser(CashFlowDbContext _dbContext, ITokenProvider _tokenProvider) : ILoggedUser
  {
    public async Task<User> Get()
    {
      string token = _tokenProvider.TokenOnRequest();

      var tokenHandler = new JwtSecurityTokenHandler();

      var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

      var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

      return await _dbContext
        .Users
        .AsNoTracking()
        .FirstAsync(user => user.UserIdIdentifier == Guid.Parse(identifier));
    }
  }
}
