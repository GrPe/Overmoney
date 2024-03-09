namespace Overmoney.Api.DataAccess.Currencies.Models;

public sealed class CreateCurrency
{
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;

    public CreateCurrency(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
