using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.DataAccess.Transactions;

internal sealed class AttachmentEntity
{
    public AttachmentId Id { get; private set; } = null!;
    public TransactionId TransactionId { get; private set; } = null!;
    public TransactionEntity Transaction { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string FilePath { get; private set; } = null!;

    public AttachmentEntity(TransactionEntity transaction, string name, string filePath)
    {
        Transaction = transaction;
        Name = name;
        FilePath = filePath;
    }

    private AttachmentEntity()
    {

    }

    public void Update(string name)
    {
        Name = name;
    }
}

internal sealed class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<AttachmentEntity>
{
    public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
    {
        builder
            .ToTable("attachments")
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => new AttachmentId(x))
            .IsRequired()
            .UseIdentityAlwaysColumn();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.FilePath)
            .IsRequired();

        builder
            .HasOne(x => x.Transaction)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.TransactionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
