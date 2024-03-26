using Overmoney.Api.Features;
using Overmoney.Api.Features.Budgets.Models;

namespace Overmoney.Api.DataAccess.Budgets;

public interface IBudgetRepository : IRepository
{
    Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Budget?> GetAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(Budget budget, CancellationToken cancellationToken);
}

public class BudgetRepository : IBudgetRepository
{
    public Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Budget?> GetAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Budget budget, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
