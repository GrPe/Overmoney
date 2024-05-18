using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.DataAccess.Users;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Categories;

internal sealed class CategoryEntity
{
    public CategoryId Id { get; private set; } = null!;
    public UserProfileId UserId { get; private set; } = null!;
    public UserProfileEntity User { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public CategoryEntity(UserProfileEntity user, string name)
    {
        User = user;
        Name = name;
    }

    public void Update(UserProfileEntity user, string name)
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
            .Property(e => e.Id)
            .HasConversion(
                x => x.Value,
                x => new CategoryId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
