using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTests;

[Collection("Infrastructure")]
public class CurrencyTests
{
    readonly HttpClient _client;

    public CurrencyTests(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
    }

    [Fact]
    public async Task When_correct_data_are_provided_currency_should_be_created()
    {
        var currency = DataFaker.GenerateCurrency();

        var response = await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content.ReadFromJsonAsync<CurrencyResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task When_currency_with_same_code_exists_error_400_should_be_throw()
    {
        var currency = DataFaker.GenerateCurrency();

        var response = await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        response.IsSuccessStatusCode.Should().BeTrue();

        var response2 = await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_currency_exists_and_update_request_is_sent_then_currency_should_be_updated()
    {
        var currency = DataFaker.GenerateCurrency();

        var response = await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        var content = await response.Content.ReadFromJsonAsync<CurrencyResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);

        var putResponse = await _client
            .PutAsJsonAsync($"currencies", new { content.Id, Name = "Updated", currency.Code });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();

        var currencyResponse = await _client
            .GetFromJsonAsync<CurrencyResponse>($"currencies/{content.Id}");

        currencyResponse.ShouldNotBeNull();
        currencyResponse.Name.ShouldBe("Updated");
    }

    [Fact]
    public async Task When_currency_exists_then_get_all_method_should_return_list_of_currencies()
    {
        var currency = DataFaker.GenerateCurrency();

        await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        currency = DataFaker.GenerateCurrency();

        await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        currency = DataFaker.GenerateCurrency();

        await _client
            .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

        var currencies = await _client
            .GetFromJsonAsync<List<CurrencyResponse>>("currencies");

        currencies.ShouldNotBeNull();
        currencies.Count.ShouldBeGreaterThan(2);
    }
}

file class CurrencyResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
}
