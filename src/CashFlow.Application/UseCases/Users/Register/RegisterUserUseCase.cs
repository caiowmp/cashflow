using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register
{
  public class RegisterUserUseCase(
    IMapper _mapper, 
    IPasswordEncripter _passwordEncripter, 
    IUserReadOnlyRepository _userReadOnlyRepository,
    IUserWriteOnlyRepository _userWriteOnlyRepository,
    IUnityOfWork _unityOfWork,
    IAcessTokenGenerator _acessToken) 
    : IRegisterUserUseCase
  {
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
      await Validate(request);

      var user = _mapper.Map<User>(request);
      user.Password = _passwordEncripter.Encrypt(request.Password);
      user.UserIdIdentifier = Guid.NewGuid();

      var acessToken = _acessToken.Generate(user);

      await _userWriteOnlyRepository.Add(user);

      await _unityOfWork.Commit();

      return new ResponseRegisteredUserJson
      {
        Name = user.Name,
        Token = acessToken,
      };
    }

    public async Task Validate(RequestRegisterUserJson request)
    {
      var result = new RegisterUserValidator().Validate(request);

      var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

      if (emailExist)
        result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

      if (!result.IsValid)
      {
        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
      }
    }
  }
}
