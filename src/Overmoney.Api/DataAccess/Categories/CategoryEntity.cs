using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.DataAccess.Categories;

internal sealed class CategoryEntity
{
    public int Id { get; init; }
    public int UserId { get; private set; }
    public UserEntity User { get; init; }
    public string Name { get; init; } = null!;

    public CategoryEntity(int id, UserEntity user, string name)
    {
        Id = id;
        User = user;
        Name = name;
    }
}

internal sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder
            .ToTable("categories")
            .HasKey(e => e.Id);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}
