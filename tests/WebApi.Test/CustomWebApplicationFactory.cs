using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
  public class CustomWebApplicationFactory : WebApplicationFactory<Program>
  {

    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public ExpenseIdentityManager Expense { get; private set; } = default!;

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

          StartDatabase(dbContext, passwordEncripter, tokenGenerator);
        });
    }

    private void StartDatabase(
      CashFlowDbContext dbContext,
      IPasswordEncripter passwordEncripter,
      IAcessTokenGenerator acessTokenGenerator)
    {

      var user = AddUserTeamMember(dbContext, passwordEncripter, acessTokenGenerator);
      AddExpenses(dbContext, user);

      dbContext.SaveChanges();
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
      var expense = ExpenseBuilder.Build(user);

      dbContext.Expenses.Add(expense);

      Expense = new ExpenseIdentityManager(expense);
    }

    private User AddUserTeamMember(
      CashFlowDbContext dbContext,
      IPasswordEncripter passwordEncripter,
      IAcessTokenGenerator acessTokenGenerator)
    {
      var user = UserBuilder.Build();
      var password = user.Password;

      user.Password = passwordEncripter.Encrypt(user.Password);

      dbContext.Users.Add(user);

      var token = acessTokenGenerator.Generate(user);

      User_Team_Member = new UserIdentityManager(user, password, token);

      return user;
    }
  }
}
