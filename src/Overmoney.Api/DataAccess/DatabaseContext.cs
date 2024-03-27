using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess.Budgets;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;

namespace Overmoney.Api.DataAccess;

internal sealed class DatabaseContext : DbContext
{
    public DbSet<WalletEntity> Wallets { get; set; }
    public DbSet<UserEntity> Users { get; set; }
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
