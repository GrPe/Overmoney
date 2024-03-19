﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.DataAccess.Wallets;

internal sealed class WalletEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public UserEntity User { get; private set; }
    public string Name { get; private set; } = null!;
    public int CurrencyId { get; private set; }
    public CurrencyEntity Currency { get; private set; }

    public WalletEntity(UserEntity user, string name, CurrencyEntity currency)
    {
        User = user;
        Name = name;
        Currency = currency;
    }

    public void Update(string name, CurrencyEntity currency, UserEntity user)
    {
        User = user;
        Currency = currency;
        Name = name;
    }
}

internal sealed class WalletEntityTypeConfiguration : IEntityTypeConfiguration<WalletEntity>
{
    public void Configure(EntityTypeBuilder<WalletEntity> builder)
    {
        builder
            .ToTable("wallets")
            .HasKey(t => t.Id);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}