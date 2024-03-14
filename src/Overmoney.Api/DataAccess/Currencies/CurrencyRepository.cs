using Overmoney.Api.Features;
using Overmoney.Api.Features.Currencies.Models;

namespace Overmoney.Api.DataAccess.Currencies;

public interface ICurrencyRepository : IRepository
{
    Task<CurrencyEntity?> GetAsync(int id, CancellationToken cancellationToken);
    Task<CurrencyEntity?> GetAsync(string code, CancellationToken cancellationToken);
    Task<IEnumerable<CurrencyEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<CurrencyEntity> CreateAsync(CreateCurrency currency, CancellationToken cancellationToken);
    Task UpdateAsync(CurrencyEntity currency, CancellationToken cancellationToken);
}

public sealed class CurrencyRepository : ICurrencyRepository
{
    private static readonly List<CurrencyEntity> _connection = [new(1, "TNT", "dummy currency")];

    public async Task<CurrencyEntity> CreateAsync(CreateCurrency currency, CancellationToken cancellationToken)
    {
        _connection.Add(new CurrencyEntity(_connection.Max(x => x.Id) + 1, currency.Code, currency.Name));
        return await Task.FromResult(_connection.Last());
    }

    public async Task<IEnumerable<CurrencyEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.AsEnumerable());
    }

    public async Task<CurrencyEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public async Task<CurrencyEntity?> GetAsync(string code, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Code == code));
    }

    public Task UpdateAsync(CurrencyEntity currency, CancellationToken cancellationToken)
    {
        var old = _connection.First(x => x.Id == currency.Id);
        _connection.Remove(old);
        _connection.Add(currency);
        return Task.CompletedTask;
    }
}
