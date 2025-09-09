using CashFlow.Application.UseCases.Users.Register;
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
