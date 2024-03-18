using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.DataAccess.Transactions;

internal class TransactionEntity
{
    public long Id { get; init; }
    public int WalletId { get; private set; }
    public WalletEntity Wallet { get; init; }
    public int UserId { get; private set; }
    public UserEntity User { get; init; }
    public int PayeeId { get; private set; }
    public PayeeEntity Payee { get; init; }
    public int CategoryId { get; private set; }
    public CategoryEntity Category { get; init; }
    public DateTime TransactionDate { get; init; }
    public TransactionType TransactionType { get; init; }
    public string? Note { get; init; }
    public double Amount { get; init; }

    public TransactionEntity(
        long id, 
        WalletEntity wallet, 
        UserEntity user, 
        PayeeEntity payee, 
        CategoryEntity category, 
        DateTime transactionDate, 
        TransactionType transactionType, 
        string? note, 
        double amount)
    {
        Id = id;
        Wallet = wallet;
        User = user;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
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
            .HasOne(x => x.Wallet)
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();

        builder
            .HasOne(x => x.Payee)
            .WithMany()
            .HasForeignKey(x => x.PayeeId)
            .IsRequired();
    }
}