using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overmoney.Api.DataAccess.Users;

internal sealed class UserEntity
{
    public int Id { get; private set; }
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
