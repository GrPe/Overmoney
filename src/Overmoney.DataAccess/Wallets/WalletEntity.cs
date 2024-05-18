using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Currencies;
using Overmoney.DataAccess.Users;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.DataAccess.Wallets;

internal sealed class WalletEntity
{
    public WalletId Id { get; private set; } = null!;
    public UserProfileId UserId { get; private set; } = null!;
    public UserProfileEntity User { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int CurrencyId { get; private set; }
    public CurrencyEntity Currency { get; private set; } = null!;

    public WalletEntity(UserProfileEntity user, string name, CurrencyEntity currency)
    {
        User = user;
        Name = name;
        Currency = currency;
    }

    public void Update(string name, CurrencyEntity currency, UserProfileEntity user)
    {
        User = user;
        Currency = currency;
        Name = name;
    }

    private WalletEntity()
    {

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
            .Property(t => t.Id)
            .HasConversion(
                x => x.Value,
                x => new WalletId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(t => t.Name)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}