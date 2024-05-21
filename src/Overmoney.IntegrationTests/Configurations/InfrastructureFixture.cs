using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Overmoney.IntegrationTests.ControllerTestCollections;
using Overmoney.IntegrationTests.Models;
using System.Net.Http.Json;
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

    UserContext[]? _userIds;
    int[]? _currencyIds;
    readonly Dictionary<int, int[]> _categoryIds = [];
    readonly Dictionary<int, int[]> _payeeIds = [];
    readonly Dictionary<int, int[]> _walletIds = [];

    public async Task InitializeAsync()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithPortBinding(POSTGRES_PORT, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
            .WithUsername("dev")
            .WithPassword("dev")
            .Build();

        await _postgresContainer
            .StartAsync();

        _application = new ApiWebApplicationFactory(_postgresContainer.Hostname, _postgresContainer.GetMappedPublicPort(POSTGRES_PORT));
        _client = _application.CreateClient();

        await SetupUsers();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _application?.Dispose();
        await _postgresContainer.StopAsync();
    }

    public HttpClient GetClient() => _client;

    public HttpClient GetClientForUser(UserContext user)
    {
        var client = _application.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.Token}");
        return client;
    }

    public HttpClient GetClientForRandomUser()
    {
        var client = _application.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_userIds!.First().Token}");
        return client;
    }

    public async Task<UserContext[]> SetupUsers()
    {
        if (_userIds is null || _userIds.Length == 0)
        {
            _userIds = new UserContext[USER_COUNT];
            for (int i = 0; i < USER_COUNT;)
            {
                var user = DataFaker.GenerateUser();
                var response = await _client
                    .PostAsJsonAsync("identity/register", new { user.Email, user.Password });

                var createProfilerResponse = await _client
                    .PostAsJsonAsync("users/profile", new { user.Email });

                var loginResponse = await _client
                    .PostAsJsonAsync(
                        "identity/login?useCookies=false&useSessionCookies=false",
                        new { user.Email, user.Password });

                if (response.IsSuccessStatusCode 
                    && createProfilerResponse.IsSuccessStatusCode 
                    && loginResponse.IsSuccessStatusCode)
                {
                    var profile = await createProfilerResponse.Content.ReadFromJsonAsync<UserProfileResponse>();
                    var content = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
                    _userIds[i] = new(profile!.Id, content!.AccessToken);
                    i++;
                }
            }
        }

        return _userIds;
    }

    public UserContext GetRandomUser()
    {
        if (_userIds is null || _userIds.Length == 0)
        {
            throw new Exception("Users are not initialized");
        }
        return _userIds[Random.Shared.Next(USER_COUNT)];
    }

    public async Task<int[]> GetCurrencies()
    {
        if (_currencyIds is null || _currencyIds.Length == 0)
        {
            _currencyIds = new int[CURRENCY_COUNT];
            var client = GetClientForUser(_userIds!.First());
            for (int i = 0; i < CURRENCY_COUNT;)
            {
                var currency = DataFaker.GenerateCurrency();
                var response = await client
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

    public async Task<int[]> GetRandomCategories(UserContext user)
    {
        if(_categoryIds.TryGetValue(user.Id, out int[]? value))
        {
            return value;
        }

        var categoryIds = new int[CATEGORY_COUNT];
        var client = GetClientForUser(user);
        for (int i = 0; i < CATEGORY_COUNT;)
        {
            var category = DataFaker.GenerateCategory();
            var response = await client
                .PostAsJsonAsync("categories", new { UserId = user.Id, Name = category });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();
                categoryIds[i] = content!.Id;
                i++;
            }
        }
        _categoryIds.Add(user.Id, categoryIds);
        return categoryIds;
    }

    public async Task<int> GetRandomCategory(UserContext user)
    {
        var categoryIds = await GetRandomCategories(user);
        return categoryIds[Random.Shared.Next(CATEGORY_COUNT)];
    }

    public async Task<int[]> GetRandomPayees(UserContext user)
    {
        if(_payeeIds.TryGetValue(user.Id, out int[]? value))
        {
            return value;
        }

        var payeeIds = new int[PAYEE_COUNT];
        var client = GetClientForUser(user);
        for (int i = 0; i < PAYEE_COUNT;)
        {
            var payee = DataFaker.GeneratePayee();
            var response = await client
                .PostAsJsonAsync("payees", new { UserId = user.Id, Name = payee });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<PayeeResponse>();
                payeeIds[i] = content!.Id;
                i++;
            }
        }
        _payeeIds.Add(user.Id, payeeIds);
        return payeeIds;
    }

    public async Task<int> GetRandomPayee(UserContext user)
    {
        var payeeIds = await GetRandomPayees(user);
        return payeeIds[Random.Shared.Next(PAYEE_COUNT)];
    }

    public async Task<int[]> GetRandomWallets(UserContext user)
    {
        if(_walletIds.TryGetValue(user.Id, out int[]? value))
        {
            return value;
        }

        var walletIds = new int[WALLET_COUNT];
        var client = GetClientForUser(user);
        for (int i = 0; i < WALLET_COUNT;)
        {
            var wallet = DataFaker.GenerateWallet();
            var currencyId = await GetRandomCurrency();
            var response = await client
                .PostAsJsonAsync("wallets", new { UserId = user.Id, Name = wallet, CurrencyId = currencyId });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WalletResponse>();
                walletIds[i] = content!.Id;
                i++;
            }
        }
        _walletIds.Add(user.Id, walletIds);
        return walletIds;
    }

    public async Task<int> GetRandomWallet(UserContext user)
    {
        var walletIds = await GetRandomWallets(user);
        return walletIds[Random.Shared.Next(WALLET_COUNT)];
    }
}

[CollectionDefinition("Infrastructure")]
public class InfrastructureCollection : ICollectionFixture<InfrastructureFixture>
{
    // Should be empty
}
