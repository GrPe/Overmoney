using FluentAssertions;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.IntegrationTests.Configurations;
using Shouldly;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTests;

[Collection("Infrastructure")]
public class PayeeTests
{
    readonly HttpClient _client;

    public PayeeTests(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
    }

    [Fact]
    public async Task When_correct_data_are_provided_payee_should_be_created()
    {
        var userData = DataFaker.GenerateUser();

        var userResponse = await _client
            .PostAsJsonAsync("users/register", new { userData.UserName, userData.Email, userData.Password });

        var userId = await userResponse.Content.ReadAsStringAsync();

        var payee = DataFaker.GeneratePayee();
        var response = await _client
            .PostAsJsonAsync("payees", new { UserId = Convert.ToInt32(userId), Name = payee });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }
}

file class PayeeResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
}