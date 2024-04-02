using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Users;

internal sealed class UserEntity
{
    public UserId Id { get; private set; } = null!;
    public string Login { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;

    public UserEntity(string login, string email, string password)
    {
        Login = login;
        Email = email;
        Password = password;
    }

    private UserEntity()
    {

    }
}

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .ToTable("users")
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .HasConversion(
                x => x.Value,
                x => new UserId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(t => t.Login)
            .IsRequired();

        builder.Property(t => t.Email)
            .IsRequired();

        builder.Property(t => t.Password)
            .IsRequired();

        builder
            .HasIndex(x => x.Login)
            .IsUnique()
            .HasDatabaseName("IX_Login");

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_Email");
    }
}
