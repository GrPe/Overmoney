using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.DataAccess.Payees;

internal sealed class PayeeEntity
{
    public PayeeId Id { get; private set; } = null!;
    public int UserId { get; private set; }
    public UserEntity User { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public PayeeEntity(UserEntity user, string name)
    {
        User = user;
        Name = name;
    }

    public void Update(UserEntity user, string name)
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

        builder.Property(x => x.Id)
            .HasConversion(
            x => x.Id, 
            x => new PayeeId(x));

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
