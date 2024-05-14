using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Overmoney.IntegrationTests.ControllerTestCollections;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Testcontainers.PostgreSql;

namespace Overmoney.IntegrationTests.Configurations;
public class InfrastructureFixture : IAsyncLifetime
{
    const int POSTGRES_PORT = 5432;
    const int USER_COUNT = 10;
    const int CURRENCY_COUNT = 5;
    const int CATEGORY_COUNT = 10;
    const int PAYEE_COUNT = 10;
    const int WALLET_COUNT = 3;

    IContainer _postgresContainer = null!;
    ApiWebApplicationFactory _application = null!;
    HttpClient _client = null!;

    int[]? _userIds;
    int[]? _currencyIds;
    readonly Dictionary<int, int[]> _categoryIds = [];
    readonly Dictionary<int, int[]> _payeeIds = [];
    readonly Dictionary<int, int[]> _walletIds = [];

    public async Task InitializeAsync()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithPortBinding(POSTGRES_PORT, true)
            .WithWaitStrategy(
                Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(POSTGRES_PORT)
                .UntilMessageIsLogged(new Regex(".*database system is ready to accept connections.*\\s")))
            .WithUsername("dev")
            .WithPassword("dev")
            .Build();

        await _postgresContainer
            .StartAsync();

        _application = new ApiWebApplicationFactory(_postgresContainer.Hostname, _postgresContainer.GetMappedPublicPort(POSTGRES_PORT));
        _client = _application.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _application?.Dispose();
        await _postgresContainer.StopAsync();
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

    public async Task<int[]> GetRandomCategories(int userId)
    {
        if(_categoryIds.TryGetValue(userId, out int[]? value))
        {
            return value;
        }

        var categoryIds = new int[CATEGORY_COUNT];
        for (int i = 0; i < CATEGORY_COUNT;)
        {
            var category = DataFaker.GenerateCategory();
            var response = await _client
                .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();
                categoryIds[i] = content!.Id;
                i++;
            }
        }
        _categoryIds.Add(userId, categoryIds);
        return categoryIds;
    }

    public async Task<int> GetRandomCategory(int userId)
    {
        var categoryIds = await GetRandomCategories(userId);
        return categoryIds[Random.Shared.Next(CATEGORY_COUNT)];
    }

    public async Task<int[]> GetRandomPayees(int userId)
    {
        if(_payeeIds.TryGetValue(userId, out int[]? value))
        {
            return value;
        }

        var payeeIds = new int[PAYEE_COUNT];
        for (int i = 0; i < PAYEE_COUNT;)
        {
            var payee = DataFaker.GeneratePayee();
            var response = await _client
                .PostAsJsonAsync("payees", new { UserId = userId, Name = payee });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();
                payeeIds[i] = content!.Id;
                i++;
            }
        }
        _payeeIds.Add(userId, payeeIds);
        return payeeIds;
    }

    public async Task<int> GetRandomPayee(int userId)
    {
        var payeeIds = await GetRandomPayees(userId);
        return payeeIds[Random.Shared.Next(PAYEE_COUNT)];
    }

    public async Task<int[]> GetRandomWallets(int userId)
    {
        if(_walletIds.TryGetValue(userId, out int[]? value))
        {
            return value;
        }

        var walletIds = new int[WALLET_COUNT];
        for (int i = 0; i < WALLET_COUNT;)
        {
            var wallet = DataFaker.GenerateWallet();
            var currencyId = await GetRandomCurrency();
            var response = await _client
                .PostAsJsonAsync("wallets", new { UserId = userId, Name = wallet, CurrencyId = currencyId });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WalletResponse>();
                walletIds[i] = content!.Id;
                i++;
            }
        }
        _walletIds.Add(userId, walletIds);
        return walletIds;
    }

    public async Task<int> GetRandomWallet(int userId)
    {
        var walletIds = await GetRandomWallets(userId);
        return walletIds[Random.Shared.Next(WALLET_COUNT)];
    }
}

[CollectionDefinition("Infrastructure")]
public class InfrastructureCollection : ICollectionFixture<InfrastructureFixture>
{
    // Should be empty
}
