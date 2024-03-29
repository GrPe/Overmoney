using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.DataAccess;

public interface IWalletRepository : IRepository
{
    Task<Wallet?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Wallet>> GetByUserAsync(int userId, CancellationToken cancellationToken);
    Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}