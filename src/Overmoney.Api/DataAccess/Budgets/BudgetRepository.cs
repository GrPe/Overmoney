using Microsoft.EntityFrameworkCore;
using Overmoney.Api.Features;
using Overmoney.Api.Features.Budgets.Models;
using Overmoney.Api.Features.Categories.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.DataAccess.Budgets;

public interface IBudgetRepository : IRepository
{
    Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Budget?> GetAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(Budget budget, CancellationToken cancellationToken);
}

internal class BudgetRepository : IBudgetRepository
{
    private readonly DatabaseContext _databaseContext;

    public BudgetRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Budget> CreateAsync(Budget budget, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == budget.UserId, cancellationToken);
        var budgetEntity = new BudgetEntity(user, budget.Name, budget.Year, budget.Month);

        var categories = await _databaseContext.Categories.Where(x => x.UserId == budget.UserId).ToListAsync(cancellationToken);
        foreach (var line in budget.BudgetLines)
        {
            var category = categories.FirstOrDefault(categories => categories.Id == line.Category.Id);

            if (category is null)
            {
                throw new DomainValidationException($"Category of id: {line.Category.Id} not found");
            }

            budgetEntity.BudgetLines.Add(new BudgetLineEntity(category, line.Amount));
        }

        var entity = _databaseContext.Add(budgetEntity);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return (await GetAsync(entity.Entity.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Budgets
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Budget?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var budget = await _databaseContext
            .Budgets
            .AsNoTracking()
            .Include(x => x.BudgetLines)
            .ThenInclude(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (budget is null)
        {
            return null;
        }

        return new Budget(
            budget.Id, 
            budget.Name, 
            budget.UserId, 
            budget.Year, 
            budget.Month, 
            budget.BudgetLines.Select(x => 
                new BudgetLine(
                    x.Id, 
                    new Category(x.Category.Id, x.Category.Name), 
                    x.Amount)).ToList());
    }

    public async Task UpdateAsync(Budget budget, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Budgets
            .Include(x => x.BudgetLines)
            .SingleOrDefaultAsync(x => x.Id == budget.Id, cancellationToken);

        if(entity is null)
        {
            return;
        }

        entity.Update(budget.Name, budget.Year, budget.Month);

        foreach(var line in entity.BudgetLines)
        {
            var entityLine = budget.BudgetLines.FirstOrDefault(x => x.Id == line.Id);

            if(entityLine is null)
            {
                _databaseContext.Remove(line);
                continue;
            }
            entityLine.Update(line.Amount);
            _databaseContext.Update(entityLine);
        }

        foreach(var line in budget.BudgetLines.Where(x => x.Id == 0))
        {
            var category = await _databaseContext.Categories.SingleAsync(x => x.Id == line.Category.Id, cancellationToken);
            entity.BudgetLines.Add(new BudgetLineEntity(category, line.Amount));
        }

        _databaseContext.Update(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
