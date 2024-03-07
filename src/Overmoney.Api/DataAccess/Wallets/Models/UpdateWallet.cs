namespace Overmoney.Api.DataAccess.Wallets.Models;

public sealed class UpdateWallet
{
    public int Id { get; init; }
    public string Name { get; init; }

    public UpdateWallet(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
