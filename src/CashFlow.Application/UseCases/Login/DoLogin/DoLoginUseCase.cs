using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
  public class DoLoginUseCase(
    IUserReadOnlyRepository _repository,
    IPasswordEncripter _passwordEncripter,
    IAcessTokenGenerator _acessTokenGenerator) : IDoLoginUseCase
  {
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
      var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

      if(!_passwordEncripter.Verify(request.Password, user.Password))
        throw new InvalidLoginException();

      return new ResponseRegisteredUserJson
      {
        Name = user.Name,
        Token = _acessTokenGenerator.Generate(user)
      };
    }
  }
}
