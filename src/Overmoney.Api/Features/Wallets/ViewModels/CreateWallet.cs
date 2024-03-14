namespace Overmoney.Api.Features.Wallets.Models;

public sealed class CreateWallet
{
    public int UserId { get; init; }
    public string Name { get; init; } = null!;
    public int CurrencyId { get; init; }

    public CreateWallet(int userId, string name, int currencyId)
    {
        UserId = userId;
        Name = name;
        CurrencyId = currencyId;
    }
}
