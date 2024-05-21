using Overmoney.IntegrationTests.Configurations;
using Overmoney.IntegrationTests.Models;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class PayeeControllerTestCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;
    readonly UserContext _userContext;

    public PayeeControllerTestCollection(InfrastructureFixture fixture)
    {
        _fixture = fixture;
        _userContext = _fixture.GetRandomUser();
        _client = fixture.GetClientForUser(_userContext);
    }

    [Fact]
    public async Task When_correct_data_are_provided_payee_should_be_created()
    {
        var payee = DataFaker.GeneratePayee();
        var response = await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_api_should_return_bad_request()
    {
        var response = await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = "" });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_payee_exists_and_update_request_is_sent_then_payee_should_be_updated()
    {
        var payee = DataFaker.GeneratePayee();
        var response = await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();

        var updatedPayee = DataFaker.GeneratePayee();
        var putResponse = await _client
            .PutAsJsonAsync($"payees", new { content!.Id, UserId = _userContext.Id, Name = updatedPayee });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();

        var payeeResponse = await _client
            .GetFromJsonAsync<PayeeResponse>($"payees/{content.Id}");

        payeeResponse.ShouldNotBeNull();
        payeeResponse.Id.ShouldBe(content.Id);
        payeeResponse.Name.ShouldBe(updatedPayee);
    }

    [Fact]
    public async Task When_payees_exists_api_should_return_a_list_of_payees()
    {
        var payee = DataFaker.GeneratePayee();
        await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        payee = DataFaker.GeneratePayee();
        await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        payee = DataFaker.GeneratePayee();
        await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        var payees = await _client
            .GetFromJsonAsync<List<PayeeResponse>>($"users/{_userContext.Id}/payees");

        payees.ShouldNotBeNull();
        payees.Count.ShouldBeGreaterThan(2);
    }

    [Fact]
    public async Task When_payee_exists_and_delete_request_is_sent_then_payee_should_be_deleted()
    {
        var payee = DataFaker.GeneratePayee();
        var response = await _client
            .PostAsJsonAsync("payees", new { UserId = _userContext.Id, Name = payee });

        var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();

        var deleteResponse = await _client
            .DeleteAsync($"payees/{content!.Id}");

        deleteResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

}

class PayeeResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
}