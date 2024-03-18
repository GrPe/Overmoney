using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.DataAccess.Payees;

internal sealed class PayeeEntity
{
    public int Id { get; init; }
    public int UserId { get; private set; }
    public UserEntity User { get; init; }
    public string Name { get; init; } = null!;

    public PayeeEntity(int id, UserEntity user, string name)
    {
        Id = id;
        User = user;
        Name = name;
    }
}

internal sealed class PayeeEntityTypeConfiguration : IEntityTypeConfiguration<PayeeEntity>
{
    public void Configure(EntityTypeBuilder<PayeeEntity> builder)
    {
        builder
            .ToTable("payees")
            .HasKey(x => x.Id);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}
