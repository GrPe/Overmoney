using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests;

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
        var response = await _client
            .PostAsJsonAsync("users/register", new { UserName = "test", Email = "test@test.test", Password = "test" });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("1");
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_error_400_should_be_throw()
    {
        var response = await _client
            .PostAsJsonAsync("users/register", new { UserName = "test", Email = "not.an.test.email", Password = "test" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}