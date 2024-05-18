using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Users;

internal sealed class UserProfileEntity
{
    public UserProfileId Id { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public UserProfileEntity( string email)
    {
        Email = email;
    }

    private UserProfileEntity()
    {

    }
}

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder
            .ToTable("users")
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .HasConversion(
                x => x.Value,
                x => new UserProfileId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(t => t.Email)
            .IsRequired();

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_Email");
    }
}
