using Overmoney.Api.DataAccess.Currencies.Models;

namespace Overmoney.Api.DataAccess.Currencies;

public interface ICurrencyRepository : IRepository
{
    Task<Currency> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken);
    Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken);
    Task<Currency> UpdateAsync(Currency currency, CancellationToken cancellationToken);
}

public sealed class CurrencyRepository : ICurrencyRepository
{
    private static readonly List<Currency> _connection = [];

    public Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Currency> GetAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Currency> UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
