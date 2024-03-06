namespace Overmoney.Api.DataAccess.Wallets.Models;

public sealed class Wallet
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public Wallet(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
