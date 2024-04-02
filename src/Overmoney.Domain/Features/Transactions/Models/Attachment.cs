using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Transactions.Models;

[JsonConverter(typeof(LongIdentityJsonConverter))]
public sealed class AttachmentId : Identity<long>
{
    public AttachmentId(long id) : base(id)
    { }
}

public sealed class Attachment
{
    public AttachmentId? Id { get; }
    public string Name { get; private set; }
    public string FilePath { get; private set; }

    public Attachment(AttachmentId? id, string name, string filePath)
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
