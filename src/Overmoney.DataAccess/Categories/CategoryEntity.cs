using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Users;

namespace Overmoney.DataAccess.Categories;

internal sealed class CategoryEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public UserEntity User { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public CategoryEntity(UserEntity user, string name)
    {
        User = user;
        Name = name;
    }

    public void Update(UserEntity user, string name)
    {
        User = user;
        Name = name;
    }

    private CategoryEntity()
    {

    }
}

internal sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder
            .ToTable("categories")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
