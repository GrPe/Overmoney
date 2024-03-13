namespace Overmoney.Api.Features.Payees.Models;

public sealed class Payee
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public Payee(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
