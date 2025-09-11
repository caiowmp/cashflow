using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
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
    private User _user;
    private string _password;

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
          var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

          SatarDatabase(dbContext, passwordEncrypter);
        });
    }

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _password;

    private void SatarDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncrypter)
    {
      _user = UserBuilder.Build();
      _password = _user.Password;

      _user.Password = passwordEncrypter.Encrypt(_user.Password);

      dbContext.Users.Add(_user);

      dbContext.SaveChanges();  
    }
  }
}
