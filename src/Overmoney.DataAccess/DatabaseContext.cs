using Microsoft.EntityFrameworkCore;
using Overmoney.DataAccess.Budgets;
using Overmoney.DataAccess.Categories;
using Overmoney.DataAccess.Currencies;
using Overmoney.DataAccess.Payees;
using Overmoney.DataAccess.Transactions;
using Overmoney.DataAccess.Users;
using Overmoney.DataAccess.Wallets;

namespace Overmoney.Api.DataAccess;

internal sealed class DatabaseContext : DbContext
{
    public DbSet<WalletEntity> Wallets { get; set; }
    public DbSet<UserProfileEntity> Users { get; set; }
    public DbSet<PayeeEntity> Payees { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CurrencyEntity> Currencies { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<RecurringTransactionEntity> RecurringTransactions { get; set; }
    public DbSet<AttachmentEntity> Attachments { get; set; }
    public DbSet<BudgetEntity> Budgets { get; set; }
    public DbSet<BudgetLineEntity> BudgetLines { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("overmoney_api");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
