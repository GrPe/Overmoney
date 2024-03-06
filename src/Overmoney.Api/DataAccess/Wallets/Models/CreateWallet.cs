namespace Overmoney.Api.DataAccess.Wallets.Models;

public sealed class CreateWallet
{
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public CreateWallet(int userId, string name)
    {
        UserId = userId;
        Name = name;
    }
}
