using Overmoney.Api.DataAccess.Currencies.Models;

namespace Overmoney.Api.DataAccess.Currencies;

public interface ICurrencyRepository : IRepository
{
    Task<Currency?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Currency?> GetAsync(string code, CancellationToken cancellationToken);
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken);
    Task<Currency> CreateAsync(CreateCurrency currency, CancellationToken cancellationToken);
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken);
}

public sealed class CurrencyRepository : ICurrencyRepository
{
    private static readonly List<Currency> _connection = [new (1, "TNT", "dummy currency")];

    public async Task<Currency> CreateAsync(CreateCurrency currency, CancellationToken cancellationToken)
    {
        _connection.Add(new Currency(_connection.Max(x => x.Id) + 1, currency.Code, currency.Name));
        return await Task.FromResult(_connection.Last());
    }

    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.AsEnumerable());
    }

    public async Task<Currency?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public async Task<Currency?> GetAsync(string code, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Code == code));
    }

    public Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var old = _connection.First(x => x.Id == currency.Id);
        _connection.Remove(old);
        _connection.Add(currency);
        return Task.CompletedTask;
    }
}
