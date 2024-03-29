namespace Overmoney.Domain.Features.Transactions.Models;

public sealed class Attachment
{
    public long? Id { get; }
    public string Name { get; private set; }
    public string FilePath { get; private set; }

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

    public void Update(string name)
    {
        Name = name;
    }
}
