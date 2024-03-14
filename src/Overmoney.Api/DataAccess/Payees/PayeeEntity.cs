namespace Overmoney.Api.DataAccess.Payees;

public sealed class PayeeEntity
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public PayeeEntity(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
