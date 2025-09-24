using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
  internal class UserRepository(CashFlowDbContext _dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
  {
    public async Task Add(User user)
    {
      await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
      return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(long id)
    {
      return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
      return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user)
    {
      _dbContext.Users.Update(user);
    }
  }
}
