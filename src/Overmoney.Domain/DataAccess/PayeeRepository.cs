using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface IPayeeRepository : IRepository
{
    Task<Payee?> GetAsync(PayeeId id, CancellationToken cancellationToken);
    Task<IEnumerable<Payee>> GetAllByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<Payee> CreateAsync(Payee payee, CancellationToken cancellationToken);
    Task UpdateAsync(Payee payee, CancellationToken cancellationToken);
    Task DeleteAsync(PayeeId id, CancellationToken cancellationToken);
}