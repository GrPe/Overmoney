using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Users;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Payees;

internal sealed class PayeeEntity
{
    public PayeeId Id { get; private set; } = null!;
    public UserProfileId UserId { get; private set; } = null!;
    public UserProfileEntity User { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public PayeeEntity(UserProfileEntity user, string name)
    {
        User = user;
        Name = name;
    }

    public void Update(UserProfileEntity user, string name)
    {
        Name = name;
        User = user;
    }

    private PayeeEntity()
    {

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
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new PayeeId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(x => x.Name)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
