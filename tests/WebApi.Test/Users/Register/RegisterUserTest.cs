using System.Net;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Test.Users.Register
{
  public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
  {
    private const string METHOD = "api/User";

    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
      _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Sucess()
    {
      var request = RequestRegisterUserJsonBuilder.Build();

      var result = await _httpClient.PostAsJsonAsync(METHOD, request);

      result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
  }
}
