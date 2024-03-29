using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Currencies.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.DataAccess.Wallets;

internal sealed class WalletRepository : IWalletRepository
{
    private readonly DatabaseContext _databaseContext;

    public WalletRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == wallet.UserId, cancellationToken);
        var currency = await _databaseContext.Currencies.SingleAsync(x => x.Id == wallet.Currency.Id, cancellationToken);

        var entity = _databaseContext.Add(new WalletEntity(user, wallet.Name, currency));
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new Wallet(entity.Entity.Id, entity.Entity.Name, new Currency(currency.Id, currency.Code, currency.Name), entity.Entity.UserId);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Wallets
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Wallets
            .Include(x => x.Currency)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Wallet(entity.Id, entity.Name, new Currency(entity.Currency.Id, entity.Currency.Code, entity.Currency.Name), entity.UserId);
    }

    public async Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await _databaseContext.Wallets
            .AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.UserId == userId)
            .Select(x => new Wallet(x.Id, x.Name, new Currency(x.Currency.Id, x.Currency.Code, x.Currency.Name), x.UserId))
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Wallet updateWallet, CancellationToken cancellationToken)
    {
        var wallet = await _databaseContext
            .Wallets
            .Include(x => x.Currency)
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == updateWallet.Id, cancellationToken);

        if (wallet is null)
        {
            return;
        }

        var user = wallet.UserId == updateWallet.UserId
            ? wallet.User
            : await _databaseContext.Users.SingleAsync(x => x.Id == wallet.UserId, cancellationToken);

        var currency = wallet.CurrencyId == updateWallet.Currency.Id
            ? wallet.Currency
            : await _databaseContext.Currencies.SingleAsync(x => x.Id == wallet.CurrencyId, cancellationToken);

        wallet.Update(updateWallet.Name, currency, user);
        _databaseContext.Update(wallet);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
