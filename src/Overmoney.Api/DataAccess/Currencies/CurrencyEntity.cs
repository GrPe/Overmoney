using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overmoney.Api.DataAccess.Currencies;

internal sealed class CurrencyEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public CurrencyEntity(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public void Update(string code, string name)
    {
        Code = code;
        Name = name;
    }

    private CurrencyEntity()
    {
        
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