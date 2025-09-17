using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
  public class CustomWebApplicationFactory : WebApplicationFactory<Program>
  {
    private Expense _expense;
    private User _user;
    private string _password;
    private string _token;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.UseEnvironment("Test")
        .ConfigureServices(services =>
        {
          var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

          services.AddDbContext<CashFlowDbContext>(ConfigureWebHost =>
          {
            ConfigureWebHost.UseInMemoryDatabase("InMmemoryDbForTesting");
            ConfigureWebHost.UseInternalServiceProvider(provider);
          });

          var scope = services.BuildServiceProvider().CreateScope();
          var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
          var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
          var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAcessTokenGenerator>();

          StartDatabase(dbContext, passwordEncripter);

          _token = tokenGenerator.Generate(_user);
        });
    }

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _password;
    public string GetToken() => _token;
    public long GetExpenseId() => _expense.Id;

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
      AddUsers(dbContext, passwordEncripter);
      AddExpenses(dbContext, _user);

      dbContext.SaveChanges();
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
      _expense = ExpenseBuilder.Build(user);

      dbContext.Expenses.Add(_expense);
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
      _user = UserBuilder.Build();
      _password = _user.Password;

      _user.Password = passwordEncripter.Encrypt(_user.Password);

      dbContext.Users.Add(_user);
    }
  }
}
