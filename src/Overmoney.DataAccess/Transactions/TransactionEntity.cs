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

internal class TransactionEntity
{
    public TransactionId Id { get; private set; } = null!;
    public WalletId WalletId { get; private set; } = null!;
    public WalletEntity Wallet { get; private set; } = null!;
    public UserProfileId UserId { get; private set; } = null!;
    public UserProfileEntity User { get; private set; } = null!;
    public PayeeId PayeeId { get; private set; } = null!;
    public PayeeEntity Payee { get; private set; } = null!;
    public CategoryId CategoryId { get; private set; } = null!;
    public CategoryEntity Category { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string? Note { get; private set; }
    public decimal Amount { get; private set; }
    public ICollection<AttachmentEntity> Attachments { get; private set; } = [];

    public TransactionEntity(
        WalletEntity wallet,
        UserProfileEntity user,
        PayeeEntity payee,
        CategoryEntity category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        decimal amount)
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
        decimal amount)
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
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new TransactionId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

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