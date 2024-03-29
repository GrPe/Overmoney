using Overmoney.Domain.Features.Budgets.Models;

namespace Overmoney.Domain.DataAccess;

public interface IBudgetRepository : IRepository
{
    Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Budget?> GetAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(Budget budget, CancellationToken cancellationToken);
}