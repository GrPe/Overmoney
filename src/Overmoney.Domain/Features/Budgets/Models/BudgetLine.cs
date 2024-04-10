using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Common.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Budgets.Models;

[JsonConverter(typeof(LongIdentityJsonConverter))]
public sealed class BudgetLineId : Identity<long>
{
    public BudgetLineId(long id = 0) : base(id)
    {
    }

    public bool IsExists() => Value != 0;
}

public sealed class BudgetLine
{
    public BudgetLineId Id { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public decimal Amount { get; private set; }

    public BudgetLine(BudgetLineId? id, Category category, decimal amount)
    {
        Id = id ?? new BudgetLineId();
        Category = category;
        Amount = amount;
    }

    public BudgetLine(Category category, decimal amount)
    {
        Category = category;
        Amount = amount;
    }

    public void Update(decimal amount)
    {
        Amount = amount;
    }
}
