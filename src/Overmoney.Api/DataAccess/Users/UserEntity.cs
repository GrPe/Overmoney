using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.DataAccess.Users;

internal sealed class UserEntity
{
    public int Id { get; init; }
    public string Login { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public UserEntity(int id, string login, string email, string password)
    {
        Id = id;
        Login = login;
        Email = email;
        Password = password;
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
            .HasIndex(x => x.Login)
            .IsUnique()
            .HasDatabaseName("IX_Login");

        builder
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_Email");
    }
}
