using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.DataAccess.Payees;

internal sealed class PayeeEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public UserEntity User { get; private set; }
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
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
