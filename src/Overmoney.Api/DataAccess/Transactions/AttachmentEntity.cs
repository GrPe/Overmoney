namespace Overmoney.Api.DataAccess.Transactions;

internal sealed class AttachmentEntity
{
    public long Id { get; init; }
    public long TransactionId {  get; init; }
    public string Name {  get; init; }
    public string FilePath { get; init; }

    public AttachmentEntity(long id, long transactionId, string name, string filePath)
    {
        Id = id;
        TransactionId = transactionId;
        Name = name;
        FilePath = filePath;
    }
}
