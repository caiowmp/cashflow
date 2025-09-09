using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin
{
  public class DoLoginUseCaseTest
  {
    [Fact]
    public async Task Sucess()
    {
      var user = UserBuilder.Build();

      var request = RequestLoginJsonBuilder.Build();

      var useCase = CreateUseCase(user);

      var result = await useCase.Execute(request);

      result.Name.Should().NotBeNull();
      result.Name.Should().Be(user.Name);
      result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {

    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {

    }

    private DoLoginUseCase CreateUseCase(User user)
    {
      var passwordEncirpter = PasswordEncripterBuilder.Build();
      var tokenGenerator = JwtTokenGeneratorBuilder.Build();
      var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();

      return new DoLoginUseCase(readRepository, passwordEncirpter, tokenGenerator);
    }
  }
}
