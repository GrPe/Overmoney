namespace Overmoney.Api.DataAccess.Wallets.Models;

public sealed class UpdateWallet
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int CurrencyId { get; init; }

    public UpdateWallet(int id, string name, int currencyId)
    {
        Id = id;
        Name = name;
        CurrencyId = currencyId;
    }
}
