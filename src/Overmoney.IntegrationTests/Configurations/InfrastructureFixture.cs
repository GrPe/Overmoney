using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Overmoney.IntegrationTests.ControllerTests;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.Configurations;
public class InfrastructureFixture : IDisposable
{
    const int POSTGRES_PORT = 5432;
    const int USER_COUNT = 10;
    const int CURRENCY_COUNT = 5;

    readonly IContainer _postgresContainer;
    readonly ApiWebApplicationFactory _application;
    readonly HttpClient _client;

    int[]? _userIds;
    int[]? _currencyIds;

    public InfrastructureFixture()
    {
        _postgresContainer = new ContainerBuilder()
            .WithImage("postgres")
            .WithPortBinding(POSTGRES_PORT, true)
            .WithEnvironment("POSTGRES_USER", "dev")
            .WithEnvironment("POSTGRES_PASSWORD", "dev")
            .Build();

        _postgresContainer.StartAsync().GetAwaiter().GetResult();
        _application = new ApiWebApplicationFactory(_postgresContainer.GetMappedPublicPort(POSTGRES_PORT));
        _client = _application.CreateClient();
    }

    public HttpClient GetClient() => _client;

    public async Task<int[]> GetUsers()
    {
        if (_userIds is null || _userIds.Length == 0)
        {
            _userIds = new int[USER_COUNT];
            for (int i = 0; i < USER_COUNT;)
            {
                var user = DataFaker.GenerateUser();
                var response = await _client
                    .PostAsJsonAsync("users/register", new { user.UserName, user.Email, user.Password });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _userIds[i] = Convert.ToInt32(content);
                    i++;
                }
            }
        }

        return _userIds;
    }

    public async Task<int> GetRandomUser()
    {
        if (_userIds is null || _userIds.Length == 0)
        {
            _userIds = await GetUsers();
        }
        return _userIds[Random.Shared.Next(USER_COUNT)];
    }

    public async Task<int[]> GetCurrencies()
    {
        if (_currencyIds is null || _currencyIds.Length == 0)
        {
            _currencyIds = new int[CURRENCY_COUNT];
            for (int i = 0; i < CURRENCY_COUNT;)
            {
                var currency = DataFaker.GenerateCurrency();
                var response = await _client
                    .PostAsJsonAsync("currencies", new { currency.Name, currency.Code });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CurrencyResponse>();
                    _currencyIds[i] = content!.Id;
                    i++;
                }
            }
        }

        return _currencyIds;
    }

    public async Task<int> GetRandomCurrency()
    {
        if(_currencyIds is null || _currencyIds.Length == 0)
        {
            _currencyIds = await GetCurrencies();
        }
        return _currencyIds[Random.Shared.Next(CURRENCY_COUNT)];
    }

    public void Dispose()
    {
        _client.Dispose();
        _application.Dispose();
        _postgresContainer.StopAsync().GetAwaiter().GetResult();
        _postgresContainer.DisposeAsync().GetAwaiter().GetResult();
    }
}

[CollectionDefinition("Infrastructure")]
public class InfrastructureCollection : ICollectionFixture<InfrastructureFixture>
{
    // Should be empty
}
