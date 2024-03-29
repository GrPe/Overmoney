using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Currencies.Models;

namespace Overmoney.DataAccess.Currencies;

internal sealed class CurrencyRepository : ICurrencyRepository
{
    private readonly DatabaseContext _databaseContext;

    public CurrencyRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Currency> CreateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var entity = _databaseContext.Currencies.Add(new CurrencyEntity(currency.Code, currency.Name));
        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new Currency(entity.Entity.Id, entity.Entity.Code, entity.Entity.Name);
    }

    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _databaseContext
            .Currencies
            .AsNoTracking()
            .Select(x => new Currency(x.Id, x.Code, x.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Currency?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Currencies
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return await Task.FromResult(new Currency(entity.Id, entity.Code, entity.Name));
    }

    public async Task<Currency?> GetAsync(string code, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Currencies
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Code == code, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return new Currency(entity.Id, entity.Code, entity.Name);
    }

    public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Currencies
            .SingleAsync(x => x.Id == currency.Id, cancellationToken);

        if (entity == null)
        {
            return;
        }

        entity.Update(currency.Code, currency.Name);
        _databaseContext.Update(entity);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
