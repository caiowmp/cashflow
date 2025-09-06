using System.Runtime.CompilerServices;
using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Register
{
  public class RegisterUserUseCase(IMapper _mapper) : IRegisterUserUseCase
  {
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
      Validate(request);

      var user = _mapper.Map<User>(request);

      return new ResponseRegisteredUserJson
      {
        Name = user.Name,
      };
    }

    public void Validate(RequestRegisterUserJson request)
    {
      var result = new RegisterUserValidator().Validate(request);

      if (!result.IsValid)
      {
        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
      }
    }
  }
}
