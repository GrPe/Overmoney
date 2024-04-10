using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Categories;
using Overmoney.Domain.Features.Budgets.Models;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.DataAccess.Budgets;

internal sealed class BudgetLineEntity
{
    public BudgetLineId Id { get; private set; } = null!;
    public BudgetId BudgetId { get; private set; } = null!;
    public BudgetEntity Budget { get; private set; } = null!;
    public CategoryId CategoryId { get; private set; } = null!;
    public CategoryEntity Category { get; private set; } = null!;
    public decimal Amount { get; private set; }

    public BudgetLineEntity(BudgetEntity budget, CategoryEntity category, decimal amount)
    {
        Budget = budget;
        Category = category;
        Amount = amount;
    }

    public BudgetLineEntity(CategoryEntity category, decimal amount)
    {
        Category = category;
        Amount = amount;
    }

    public void Update(decimal amount)
    {
        Amount = amount;
    }

    private BudgetLineEntity()
    {

    }
}

internal sealed class BudgetLineEntityTypeConfiguration : IEntityTypeConfiguration<BudgetLineEntity>
{
    public void Configure(EntityTypeBuilder<BudgetLineEntity> builder)
    {
        builder
            .ToTable("budget_lines")
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasConversion(
                x => x.Value,
                x => new BudgetLineId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder
            .HasOne(x => x.Budget)
            .WithMany(x => x.BudgetLines)
            .HasForeignKey(x => x.BudgetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}