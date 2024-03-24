using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.DataAccess.Transactions;

internal sealed class RecurringTransactionEntity
{
    public long Id { get; private set; }
    public int WalletId { get; private set; }
    public WalletEntity Wallet { get; private set; } = null!;
    public int UserId { get; private set; }
    public UserEntity User { get; private set; } = null!;
    public int PayeeId { get; private set; }
    public PayeeEntity Payee { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public CategoryEntity Category { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string? Note { get; private set; }
    public double Amount { get; private set; }
    public Schedule Schedule { get; private set; }

    public RecurringTransactionEntity(
        WalletEntity wallet,
        UserEntity user,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
        Schedule schedule)
    {
        Wallet = wallet;
        User = user;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Schedule = schedule;
    }

    public void Update(
        WalletEntity wallet,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
        Schedule schedule)
    {
        Wallet = wallet;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
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

        builder.Property(x => x.TransactionDate)
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

        builder
            .Property(x => x.Schedule)
            .HasConversion(
                v => v.ToString(),
                v => new Schedule(v));
    }
}