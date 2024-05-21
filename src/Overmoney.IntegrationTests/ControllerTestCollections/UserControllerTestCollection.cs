using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using Overmoney.IntegrationTests.Models;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class UserControllerTestCollection
{
    readonly HttpClient _client;

    public UserControllerTestCollection(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
    }

    [Fact]
    public async Task When_correct_data_are_provided_user_should_be_created()
    {
        var user = DataFaker.GenerateUser();

        var createUserResponse = await _client
            .PostAsJsonAsync("identity/register", new { user.Email, user.Password });

        createUserResponse.IsSuccessStatusCode.Should().BeTrue();

        var response = await _client
            .PostAsJsonAsync("users/profile", new { user.Email });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();

        content.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_error_400_should_be_throw()
    {
        var response = await _client
            .PostAsJsonAsync("identity/register", new { UserName = "test", Email = "not.an.test.email", Password = "test" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_user_is_created_user_should_be_able_to_login()
    {
        var user = DataFaker.GenerateUser();

        var createUserResponse = await _client
            .PostAsJsonAsync("identity/register", new { user.Email, user.Password });

        createUserResponse.IsSuccessStatusCode.Should().BeTrue();

        var userProfile = await _client
            .PostAsJsonAsync("users/profile", new { user.Email, user.Password });

        userProfile.IsSuccessStatusCode.Should().BeTrue();

        var loginResponse = await _client
            .PostAsJsonAsync("identity/login?useCookies=false&useSessionCookies=false",
            new { user.Email, user.Password });

        var token = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        token.Should().NotBeNull();

        var request = new HttpRequestMessage(HttpMethod.Get, "users/profile");
        request.Headers.Add("Authorization", $"Bearer {token!.AccessToken}");
        var userProfileResponse = await _client.SendAsync(request);

        userProfileResponse.IsSuccessStatusCode.Should().BeTrue();
        var content = await userProfileResponse.Content.ReadAsStringAsync();

        content.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task When_user_is_created_user_should_be_to_remove_account()
    {
        var user = DataFaker.GenerateUser();

        var createUserResponse = await _client
            .PostAsJsonAsync("identity/register", new { user.Email, user.Password });

        createUserResponse.IsSuccessStatusCode.Should().BeTrue();

        var userProfile = await _client
            .PostAsJsonAsync("users/profile", new { user.Email, user.Password });

        userProfile.IsSuccessStatusCode.Should().BeTrue();

        var loginResponse = await _client
            .PostAsJsonAsync("identity/login?useCookies=false&useSessionCookies=false",
            new { user.Email, user.Password });

        var token = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        token.Should().NotBeNull();

        var userProfileContent = await userProfile.Content.ReadFromJsonAsync<UserProfileResponse>();

        var request = new HttpRequestMessage(HttpMethod.Delete, $"users/{userProfileContent!.Id}");
        request.Headers.Add("Authorization", $"Bearer {token!.AccessToken}");

        var deleteResponse = await _client.SendAsync(request);

        deleteResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}