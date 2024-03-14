namespace Overmoney.Api.Features.Payees.Models;

public sealed class Payee
{
    public int? Id { get; }
    public int UserId { get; }
    public string Name { get; }

    public Payee(int id, int userId, string name) : this(userId, name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }

    public Payee(int userId, string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        UserId = userId;
        Name = name;
    }

}
