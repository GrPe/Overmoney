using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests;

public class UserTests
{
    readonly IContainer _postgresContainer;

    public UserTests()
    {
        _postgresContainer = new ContainerBuilder()
            .WithImage("postgres")
            .WithPortBinding(5432, true)
            .WithEnvironment("POSTGRES_USER", "dev")
            .WithEnvironment("POSTGRES_PASSWORD", "dev")
            .Build();

        _postgresContainer.StartAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task When_correct_data_are_provided_user_should_be_created()
    {
        await using var application = new ApiWebApplicationFactory(_postgresContainer.GetMappedPublicPort(5432));

        using var client = application.CreateClient();

        var response = await client
            .PostAsJsonAsync("users/register", new { UserName = "test", Email = "test@test.test", Password = "test" });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("1");
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_error_400_should_be_throw()
    {
        await using var application = new ApiWebApplicationFactory(_postgresContainer.GetMappedPublicPort(5432));

        using var client = application.CreateClient();

        var response = await client
            .PostAsJsonAsync("users/register", new { UserName = "test", Email = "not.an.test.email", Password = "test" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}