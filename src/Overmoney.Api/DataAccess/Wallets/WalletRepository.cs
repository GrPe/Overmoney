using Overmoney.Api.DataAccess.Wallets.Models;

namespace Overmoney.Api.DataAccess.Wallets;

public interface IWalletRepository : IRepository
{
    Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<int> CreateAsync(CreateWallet wallet, CancellationToken cancellationToken);
}

public sealed class WalletRepository : IWalletRepository
{
    private readonly List<Wallet> _connection = [new(1, 1, "My wallet"), new(2, 1, "test")];

    public async Task<int> CreateAsync(CreateWallet wallet, CancellationToken cancellationToken)
    {
        _connection.Add(new(_connection.Max(x => x.Id) + 1, wallet.UserId, wallet.Name));
        return await Task.FromResult(_connection.Max(x => x.Id));
    }

    public async Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public async Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId));
    }
}
