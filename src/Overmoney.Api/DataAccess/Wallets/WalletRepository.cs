using Overmoney.Api.Features;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.DataAccess.Wallets;

public interface IWalletRepository : IRepository
{
    Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class WalletRepository : IWalletRepository
{
    private static readonly List<WalletEntity> _connection = [new(1, 1, "My wallet", 1), new(2, 1, "test", 1)];

    public async Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken)
    {
        WalletEntity entity = new(_connection.Max(x => x.Id) + 1, wallet.UserId, wallet.Name, wallet.CurrencyId);
        _connection.Add(entity);
        return await Task.FromResult(new Wallet(entity.Id, entity.Name, entity.CurrencyId, entity.UserId));
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var wallet = _connection.FirstOrDefault(x => x.Id == id);
        if (wallet != null)
        {
            _connection.Remove(wallet);
        }
        return Task.CompletedTask;
    }

    public async Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = _connection.FirstOrDefault(x => x.Id == id);
        if(entity is null)
        {
            return null;
        }
        return await Task.FromResult(new Wallet(entity.Id, entity.Name, entity.CurrencyId, entity.UserId));
    }

    public async Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId).Select(x => new Wallet(x.Id, x.Name, x.CurrencyId, x.UserId)));
    }

    public Task UpdateAsync(Wallet updateWallet, CancellationToken cancellationToken)
    {
        var wallet = _connection.First(x => x.Id == updateWallet.Id);
        var newWallet = new WalletEntity(wallet.Id, wallet.UserId, updateWallet.Name, updateWallet.CurrencyId);
        _connection.Remove(wallet);
        _connection.Add(newWallet);

        return Task.CompletedTask;
    }
}
