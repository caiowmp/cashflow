using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.User
{
  internal class UserRepository(CashFlowDbContext _dbContext) : IUserReadOnlyRepository
  {
    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
      return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }
  }
}
