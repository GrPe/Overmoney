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
    public string Path { get; private set; }

    public Attachment(AttachmentId? id, string name, string path)
    {
        Id = id;
        Name = name;
        Path = path;
    }

    public Attachment(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
