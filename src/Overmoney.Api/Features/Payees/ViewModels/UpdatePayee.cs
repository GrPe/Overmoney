namespace Overmoney.Api.Features.Payees.Models;

public sealed class UpdatePayee
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public UpdatePayee(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
