using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.DataAccess.Wallets;

internal sealed class WalletEntity
{
    public int Id { get; init; }
    public int UserId { get; private set; }
    public UserEntity User { get; init; }
    public string Name { get; init; } = null!;
    public int CurrencyId { get; init; }

    public WalletEntity(int id, UserEntity user, string name, int currencyId)
    {
        Id = id;
        User = user;
        Name = name;
        CurrencyId = currencyId;
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
            .IsRequired();
    }
}