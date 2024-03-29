using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Users;

namespace Overmoney.DataAccess.Budgets;

internal sealed class BudgetEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int UserId { get; private set; }
    public UserEntity User { get; private set; } = null!;
    public int Year { get; private set; }
    public int Month { get; private set; }
    public ICollection<BudgetLineEntity> BudgetLines { get; private set; } = [];

    public BudgetEntity(UserEntity user, string name, int year, int month)
    {
        User = user;
        Name = name;
        Year = year;
        Month = month;
    }

    public void Update(string name, int year, int month)
    {
        Name = name;
        Year = year;
        Month = month;
    }

    private BudgetEntity()
    {

    }
}

internal sealed class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<BudgetEntity>
{
    public void Configure(EntityTypeBuilder<BudgetEntity> builder)
    {
        builder
            .ToTable("budgets")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
