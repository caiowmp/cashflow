namespace CashFlow.Domain.Repositories
{
  public interface IUnityOfWork
  {
    void Commit();
  }
}
