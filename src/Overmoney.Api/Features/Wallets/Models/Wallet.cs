namespace Overmoney.Api.Features.Wallets.Models;

public sealed class Wallet
{
    public int Id { get; }
    public int UserId {  get; }
    public string Name { get; }
    public int CurrencyId { get; }

    public Wallet(int id, string name, int currencyId, int userId)
        : this(name, currencyId, userId)
    {
        Id = id;
    }

    public Wallet(string name, int currencyId, int userId)
    {
        Name = name;
        CurrencyId = currencyId;
        UserId = userId;
    }
}
