using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Categories;

namespace Overmoney.Api.DataAccess.Budgets;

internal sealed class BudgetLineEntity
{
    public long Id { get; private set; }
    public int BudgetId { get; private set; }
    public BudgetEntity Budget { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public CategoryEntity Category { get; private set; } = null!;
    public double Amount { get; private set; }

    public BudgetLineEntity(BudgetEntity budget, CategoryEntity category, double amount)
    {
        Budget = budget;
        Category = category;
        Amount = amount;
    }

    public BudgetLineEntity(CategoryEntity category, double amount)
    {
        Category = category;
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