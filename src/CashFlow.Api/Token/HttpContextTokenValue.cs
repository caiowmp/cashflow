using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token
{
  public class HttpContextTokenValue(IHttpContextAccessor _httpContextAccessor) : ITokenProvider
  {
    public string TokenOnRequest()
    {
       var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
    
      return authorization["Bearer ".Length..].Trim();
    }
  }
}
