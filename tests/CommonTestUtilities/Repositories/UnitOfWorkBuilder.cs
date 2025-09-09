using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories
{
  public class UnitOfWorkBuilder
  {
    public static IUnityOfWork Build()
    {
      var mock = new Mock<IUnityOfWork>();

      return mock.Object;
    }
  }
}
