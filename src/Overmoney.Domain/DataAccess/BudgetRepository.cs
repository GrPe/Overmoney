using Overmoney.Domain.Features.Budgets.Models;

namespace Overmoney.Domain.DataAccess;

public interface IBudgetRepository : IRepository
{
    Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken);
    Task DeleteAsync(BudgetId id, CancellationToken cancellationToken);
    Task<Budget?> GetAsync(BudgetId id, CancellationToken cancellationToken);
    Task UpdateAsync(Budget budget, CancellationToken cancellationToken);
}