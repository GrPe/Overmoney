using Overmoney.Domain.Features.Currencies.Models;

namespace Overmoney.Domain.DataAccess;

public interface ICurrencyRepository : IRepository
{
    Task<Currency?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Currency?> GetAsync(string code, CancellationToken cancellationToken);
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken);
    Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken);
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken);
}