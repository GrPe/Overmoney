using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.Features.Budgets.Models;

public sealed class BudgetLine
{
    public long Id { get; private set; }
    public Category Category { get; private set; } = null!;
    public double Amount { get; private set; }

    public BudgetLine(long id, Category category, double amount)
    {
        Id = id;
        Category = category;
        Amount = amount;
    }

    public BudgetLine(Category category, double amount)
    {
        Category = category;
        Amount = amount;
    }

    public void Update(double amount)
    {
        Amount = amount;
    }
}
