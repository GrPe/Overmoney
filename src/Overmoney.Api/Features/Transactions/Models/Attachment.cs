namespace Overmoney.Api.Features.Transactions.Models;

public sealed class Attachment
{
    public long? Id { get; }
    public string Name { get; }
    public string FilePath {  get; }

    public Attachment(long? id, string name, string filePath)
    {
        Id = id;
        Name = name;
        FilePath = filePath;
    }

    public Attachment(string name, string filePath)
    {
        Name = name;
        FilePath = filePath;
    }
}
