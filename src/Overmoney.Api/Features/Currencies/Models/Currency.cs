
namespace Overmoney.Api.Features.Currencies.Models;

public sealed class Currency
{
    public int? Id { get; }
    public string Code { get; }
    public string Name { get; }

    public Currency(int id, string code, string name) : this(code, name)
    {
        Id = id;
    }

    public Currency(string code, string name)
    {
        if(string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentNullException(nameof(code));
        }

        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(code));
        }

        Code = code;
        Name = name;
    }
}
