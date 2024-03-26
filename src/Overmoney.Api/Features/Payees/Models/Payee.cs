using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Payees.Models;

public sealed record PayeeId
{
    public int Id { get; }

    public PayeeId(int id)
    {
        if(id <= 0)
        {
            throw new DomainValidationException("Id must be greater than 0");
        }
        Id = id;
    }
}

public sealed class Payee
{
    public PayeeId? Id { get; }
    public int UserId { get; }
    public string Name { get; }

    public Payee(PayeeId id, int userId, string name) : this(userId, name)
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
