using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Categories;
using Overmoney.DataAccess.Payees;
using Overmoney.DataAccess.Users;
using Overmoney.DataAccess.Wallets;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.DataAccess.Transactions;

internal class TransactionEntity
{
    public long Id { get; private set; }
    public int WalletId { get; private set; }
    public WalletEntity Wallet { get; private set; } = null!;
    public int UserId { get; private set; }
    public UserEntity User { get; private set; } = null!;
    public PayeeId PayeeId { get; private set; } = null!;
    public PayeeEntity Payee { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public CategoryEntity Category { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string? Note { get; private set; }
    public double Amount { get; private set; }
    public ICollection<AttachmentEntity> Attachments { get; private set; } = [];

    public TransactionEntity(
        WalletEntity wallet,
        UserEntity user,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount)
    {
        Wallet = wallet;
        User = user;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
    }

    public void Update(
        WalletEntity wallet,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount)
    {
        Wallet = wallet;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
    }

    private TransactionEntity()
    {

    }
}

internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder
            .ToTable("transactions")
            .HasKey(x => x.Id);

        builder
            .Property(x => x.TransactionDate)
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .IsRequired();

        builder.Property(x => x.TransactionType)
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
            .HasOne(x => x.Payee)
            .WithMany()
            .HasForeignKey(x => x.PayeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}