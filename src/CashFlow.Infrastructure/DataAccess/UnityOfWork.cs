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

    public async Task Commit() => await _dbContext.SaveChangesAsync();

  }
}
