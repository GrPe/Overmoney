using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Users.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Payees.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class PayeeId : Identity<int>
{
    public PayeeId(int value)
        : base(value)
    { }
}

public sealed class Payee
{
    public PayeeId? Id { get; }
    public UserId UserId { get; } = null!;
    public string Name { get; }

    public Payee(PayeeId id, UserId userId, string name) : this(userId, name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }

    public Payee(UserId userId, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        UserId = userId;
        Name = name;
    }
}
