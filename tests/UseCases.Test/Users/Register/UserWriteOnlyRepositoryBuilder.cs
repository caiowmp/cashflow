using CashFlow.Domain.Repositories.User;
using Moq;

namespace UseCases.Test.Users.Register
{
  public class UserWriteOnlyRepositoryBuilder
  {
    public static IUserWriteOnlyRepository Build()
    {
      var mock = new Mock<IUserWriteOnlyRepository>();

      return mock.Object;
    }
  }
}
