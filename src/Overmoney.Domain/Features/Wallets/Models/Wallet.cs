using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Currencies.Models;
using Overmoney.Domain.Features.Users.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Wallets.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class WalletId : Identity<int>
{
    public WalletId(int id) : base(id)
    {
    }
}

public sealed class Wallet
{
    public WalletId? Id { get; }
    public UserProfileId UserId { get; } = null!;
    public string Name { get; }
    public Currency Currency { get; }

    public Wallet(WalletId id, string name, Currency currency, UserProfileId userId)
        : this(name, currency, userId)
    {
        Id = id;
    }

    public Wallet(string name, Currency currency, UserProfileId userId)
    {
        Name = name;
        Currency = currency;
        UserId = userId;
    }
}
