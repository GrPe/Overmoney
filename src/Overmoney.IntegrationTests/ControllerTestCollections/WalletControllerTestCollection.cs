using Overmoney.IntegrationTests.Configurations;
using Overmoney.IntegrationTests.Models;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class WalletControllerTestCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;
    readonly UserContext _userContext;

    public WalletControllerTestCollection(InfrastructureFixture fixture)
    {
        _fixture = fixture;
        _userContext = _fixture.GetRandomUser();
        _client = fixture.GetClientForUser(_userContext);
    }

    [Fact]
    public async Task When_correct_data_are_provided_wallet_should_be_created()
    {
        var currencyId = await _fixture.GetRandomCurrency();

        var wallet = DataFaker.GenerateWallet();
        var response = await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<WalletResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_api_should_return_bad_request()
    {
        var response = await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = "" });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_wallet_exists_and_update_request_is_sent_then_wallet_should_be_updated()
    {
        var currencyId = await _fixture.GetRandomCurrency();

        var wallet = DataFaker.GenerateWallet();
        var response = await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        var content = await response.Content.ReadFromJsonAsync<WalletResponse>();

        var updatedWallet = DataFaker.GenerateWallet();
        var putResponse = await _client
            .PutAsJsonAsync($"wallets", new { content!.Id, UserId = _userContext.Id, Name = updatedWallet, CurrencyId = content.Currency.Id });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task When_wallet_exists_and_delete_request_is_sent_then_wallet_should_be_deleted()
    {
        var currencyId = await _fixture.GetRandomCurrency();

        var wallet = DataFaker.GenerateWallet();
        var response = await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        var content = await response.Content.ReadFromJsonAsync<WalletResponse>();

        var deleteResponse = await _client
            .DeleteAsync($"wallets/{content!.Id}");

        deleteResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task When_wallets_exists_api_should_return_a_list_of_wallets()
    {
        var wallet = DataFaker.GenerateWallet();
        var currencyId = await _fixture.GetRandomCurrency();
        await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        wallet = DataFaker.GenerateWallet();
        currencyId = await _fixture.GetRandomCurrency();
        await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        wallet = DataFaker.GenerateWallet();
        currencyId = await _fixture.GetRandomCurrency();
        await _client
            .PostAsJsonAsync("wallets", new { UserId = _userContext.Id, Name = wallet, CurrencyId = currencyId });

        var wallets = await _client
            .GetFromJsonAsync<List<WalletResponse>>($"users/{_userContext.Id}/wallets");

        wallet.ShouldNotBeNull();
        wallets!.Count.ShouldBeGreaterThan(2);
    }
}

record WalletResponse(int Id, string Name, int UserId, CurrencyResponse Currency);