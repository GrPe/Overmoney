using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Overmoney.Api.DataAccess.Transactions;

internal sealed class AttachmentEntity
{
    public long Id { get; init; }
    public long TransactionId {  get; private set; }
    public TransactionEntity Transaction { get; init; }
    public string Name {  get; init; }
    public string FilePath { get; init; }

    public AttachmentEntity(long id, TransactionEntity transaction, string name, string filePath)
    {
        Id = id;
        Transaction = transaction;
        Name = name;
        FilePath = filePath;
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
            .HasOne(x => x.Transaction)
            .WithMany()
            .HasForeignKey(x => x.TransactionId)
            .IsRequired();
    }
}
