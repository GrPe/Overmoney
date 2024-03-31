using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Budgets.Models;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.DataAccess.Budgets;

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

    public async Task DeleteAsync(BudgetId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Budgets
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Budget?> GetAsync(BudgetId id, CancellationToken cancellationToken)
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
            budget.UserId,
            budget.Name,
            budget.Year,
            budget.Month,
            budget.BudgetLines.Select(x =>
                new BudgetLine(
                    x.Id,
                    new Category(x.Category.Id, x.Category.UserId, x.Category.Name),
                    x.Amount)).ToList());
    }

    public async Task UpdateAsync(Budget budget, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Budgets
            .Include(x => x.BudgetLines)
            .SingleOrDefaultAsync(x => x.Id == budget.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        entity.Update(budget.Name, budget.Year, budget.Month);
        _databaseContext.Update(entity);

        List<BudgetLineEntity> linesToRemove = [];
        foreach (var entityLine in entity.BudgetLines)
        {
            var line = budget.BudgetLines.FirstOrDefault(x => x.Id == entityLine.Id);

            if (line is null)
            {
                linesToRemove.Add(entityLine);
                continue;
            }
            entityLine.Update(line.Amount);
        }

        foreach (var line in linesToRemove)
        {
            entity.BudgetLines.Remove(line);
        }

        foreach (var line in budget.BudgetLines.Where(x => x.Id == 0))
        {
            var category = await _databaseContext.Categories.SingleAsync(x => x.Id == line.Category.Id, cancellationToken);
            entity.BudgetLines.Add(new BudgetLineEntity(category, line.Amount));
        }

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
