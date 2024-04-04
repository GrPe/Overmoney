using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTests;

public class UserTests : IClassFixture<InfrastructureFixture>
{
    readonly HttpClient _client;

    public UserTests(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
    }

    [Fact]
    public async Task When_correct_data_are_provided_user_should_be_created()
    {
        var user = DataFaker.GenerateUser();

        var response = await _client
            .PostAsJsonAsync("users/register", new { user.UserName, user.Email, user.Password });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();

        content.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_error_400_should_be_throw()
    {
        var response = await _client
            .PostAsJsonAsync("users/register", new { UserName = "test", Email = "not.an.test.email", Password = "test" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_user_is_created_user_should_be_able_to_login()
    {
        var user = DataFaker.GenerateUser();

        var response = await _client
            .PostAsJsonAsync("users/register", new { user.UserName, user.Email, user.Password });

        response.IsSuccessStatusCode.Should().BeTrue();

        var loginResponse = await _client
            .PostAsJsonAsync("users/login", new { Login = user.UserName, user.Password });

        loginResponse.IsSuccessStatusCode.Should().BeTrue();
        var content = await loginResponse.Content.ReadAsStringAsync();

        content.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task When_user_is_created_user_should_be_to_remove_account()
    {
        var user = DataFaker.GenerateUser();

        var response = await _client
            .PostAsJsonAsync("users/register", new { user.UserName, user.Email, user.Password });

        response.IsSuccessStatusCode.Should().BeTrue();

        var userId = await response.Content.ReadAsStringAsync();

        var loginResponse = await _client
            .DeleteAsync($"users/{userId}");

        loginResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}