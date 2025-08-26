using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess
{
  internal class UnityOfWork : IUnityOfWork
  {
    private readonly CashFlowDbContext _dbContext;

    public UnityOfWork(CashFlowDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public void Commit() => _dbContext.SaveChanges();

  }
}
