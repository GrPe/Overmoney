namespace Overmoney.Api.DataAccess.Currencies;

internal sealed class CurrencyEntity
{
    public int Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;

    public CurrencyEntity(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}
