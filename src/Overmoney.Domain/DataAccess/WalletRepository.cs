using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.DataAccess;

public interface IWalletRepository : IRepository
{
    Task<Wallet?> GetAsync(WalletId id, CancellationToken cancellationToken);
    Task<IEnumerable<Wallet>> GetByUserAsync(UserProfileId userId, CancellationToken cancellationToken);
    Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken);
    Task DeleteAsync(WalletId id, CancellationToken cancellationToken);
}