using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Categories;
using Overmoney.DataAccess.Payees;
using Overmoney.DataAccess.Users;
using Overmoney.DataAccess.Wallets;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.DataAccess.Transactions;

internal sealed class RecurringTransactionEntity
{
    public RecurringTransactionId Id { get; private set; } = null!;
    public WalletId WalletId { get; private set; } = null!;
    public WalletEntity Wallet { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public UserEntity User { get; private set; } = null!;
    public PayeeId PayeeId { get; private set; } = null!;
    public PayeeEntity Payee { get; private set; } = null!;
    public CategoryId CategoryId { get; private set; } = null!;
    public CategoryEntity Category { get; private set; } = null!;
    public DateTime NextOccurrence { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string? Note { get; private set; }
    public decimal Amount { get; private set; }
    public Schedule Schedule { get; private set; }

    public RecurringTransactionEntity(
        WalletEntity wallet,
        UserEntity user,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime nextOccurrence,
        TransactionType transactionType,
        string? note,
        decimal amount,
        Schedule schedule)
    {
        Wallet = wallet;
        User = user;
        Payee = payee;
        Category = category;
        NextOccurrence = nextOccurrence;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Schedule = schedule;
    }

    public void Update(
        WalletEntity wallet,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime nextOccurrence,
        TransactionType transactionType,
        string? note,
        decimal amount,
        Schedule schedule)
    {
        Wallet = wallet;
        Payee = payee;
        Category = category;
        NextOccurrence = nextOccurrence;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Schedule = schedule;
    }

    private RecurringTransactionEntity()
    {

    }
}

internal sealed class RecurringTransactionEntityConfiguration : IEntityTypeConfiguration<RecurringTransactionEntity>
{
    public void Configure(EntityTypeBuilder<RecurringTransactionEntity> builder)
    {
        builder
            .ToTable("recurring_transactions")
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new RecurringTransactionId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(x => x.NextOccurrence)
            .IsRequired();

        builder.Property(x => x.TransactionType)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder
            .HasOne(x => x.Wallet)
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Payee)
            .WithMany()
            .HasForeignKey(x => x.PayeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PayeeId)
            .HasConversion(
                x => x.Value,
                x => new PayeeId(x));

        builder
            .Property(x => x.Schedule)
            .HasConversion(
                v => v.Cron,
                v => new Schedule(v));
    }
}