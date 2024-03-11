namespace Overmoney.Api.DataAccess.Payees.Models;

public sealed class CreatePayee
{
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public CreatePayee(int userId, string name)
    {
        UserId = userId;
        Name = name;
    }
}
