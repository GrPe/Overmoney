using Overmoney.Api.Features;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.DataAccess.Wallets;

public interface IWalletRepository : IRepository
{
    Task<WalletEntity?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<WalletEntity>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<WalletEntity> CreateAsync(CreateWallet wallet, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateWallet updateWallet, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class WalletRepository : IWalletRepository
{
    private static readonly List<WalletEntity> _connection = [new(1, 1, "My wallet", 1), new(2, 1, "test", 1)];

    public async Task<WalletEntity> CreateAsync(CreateWallet wallet, CancellationToken cancellationToken)
    {
        _connection.Add(new(_connection.Max(x => x.Id) + 1, wallet.UserId, wallet.Name, wallet.CurrencyId));
        return await Task.FromResult(_connection.Last());
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

    public async Task<WalletEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public async Task<IEnumerable<WalletEntity>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId));
    }

    public Task UpdateAsync(UpdateWallet updateWallet, CancellationToken cancellationToken)
    {
        var wallet = _connection.First(x => x.Id == updateWallet.Id);
        var newWallet = new WalletEntity(wallet.Id, wallet.UserId, updateWallet.Name, updateWallet.CurrencyId);
        _connection.Remove(wallet);
        _connection.Add(newWallet);

        return Task.CompletedTask;
    }
}
