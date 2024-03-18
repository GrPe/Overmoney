using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overmoney.Api.DataAccess.Currencies;

internal sealed class CurrencyEntity
{
    public int Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;

    public CurrencyEntity(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}

internal sealed class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<CurrencyEntity>
{
    public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
    {
        builder
            .ToTable("currencies")
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_Code");
    }
}