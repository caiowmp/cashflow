using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register
{
  public class RegisterUserUseCaseTest
  {
    [Fact]
    public async Task Sucess()
    {
      var request = RequestRegisterUserJsonBuilder.Build();
      var useCase = CreateUseCase();

      var result = await useCase.Execute(request);

      result.Should().NotBeNull();
      result.Name.Should().Be(request.Name);
      result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
      var request = RequestRegisterUserJsonBuilder.Build();
      request.Name = string.Empty;
      
      var useCase = CreateUseCase();

      var act = async () => await useCase.Execute(request);

      var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

      result.Where(exception => exception.GetErrors().Count == 1 && exception.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    private RegisterUserUseCase CreateUseCase()
    {
      var mapper = MapperBuilder.Build();
      var passwordEncripter = PasswordEncripterBuilder.Build();
      var readRepository = new UserReadOnlyRepositoryBuilder().Build();
      var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
      var unitOfWork = UnitOfWorkBuilder.Build();
      var tokenGenerator = JwtTokenGeneratorBuilder.Build();

      return new RegisterUserUseCase(mapper, passwordEncripter, readRepository, writeRepository, unitOfWork, tokenGenerator);
    }
  }
}
