using Overmoney.Api.Features;
using Overmoney.Api.Features.Currencies.Models;

namespace Overmoney.Api.DataAccess.Currencies;

public interface ICurrencyRepository : IRepository
{
    Task<Currency?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Currency?> GetAsync(string code, CancellationToken cancellationToken);
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken);
    Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken);
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken);
}

public sealed class CurrencyRepository : ICurrencyRepository
{
    private static readonly List<CurrencyEntity> _connection = [new(1, "TNT", "dummy currency")];

    public async Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var entity = new CurrencyEntity(_connection.Max(x => x.Id) + 1, currency.Code, currency.Name);
        _connection.Add(entity);
        return await Task.FromResult(new Currency(entity.Id, entity.Code, entity.Name));
    }

    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Select(x => new Currency(x.Id, x.Code, x.Name)));
    }

    public async Task<Currency?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = _connection.FirstOrDefault(x => x.Id == id);
        if(entity == null)
        {
            return null;
        }
        return await Task.FromResult(new Currency(entity.Id, entity.Code, entity.Name));
    }

    public async Task<Currency?> GetAsync(string code, CancellationToken cancellationToken)
    {
        var entity = _connection.FirstOrDefault(x => x.Code == code);
        if (entity == null)
        {
            return null;
        }
        return await Task.FromResult(new Currency(entity.Id, entity.Code, entity.Name));
    }

    public Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var old = _connection.First(x => x.Id == currency.Id);
        _connection.Remove(old);
        _connection.Add(new CurrencyEntity(currency!.Id!.Value, currency.Code, currency.Name));
        return Task.CompletedTask;
    }
}
