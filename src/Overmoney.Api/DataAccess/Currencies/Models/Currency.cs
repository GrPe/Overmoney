namespace Overmoney.Api.DataAccess.Currencies.Models;

public sealed class Currency
{
    public int Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public double ExchangeRate { get; init; }
}
