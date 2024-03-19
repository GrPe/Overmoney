using Overmoney.Api.Features.Currencies.Models;

namespace Overmoney.Api.Features.Wallets.Models;

public sealed class Wallet
{
    public int Id { get; }
    public int UserId { get; }
    public string Name { get; }
    public Currency Currency { get; }

    public Wallet(int id, string name, Currency currency, int userId)
        : this(name, currency, userId)
    {
        Id = id;
    }

    public Wallet(string name, Currency currency, int userId)
    {
        Name = name;
        Currency = currency;
        UserId = userId;
    }
}
