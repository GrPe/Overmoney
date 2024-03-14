﻿namespace Overmoney.Api.Features.Wallets.Models;

internal sealed class WalletEntity
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;
    public int CurrencyId { get; init; }

    public WalletEntity(int id, int userId, string name, int currencyId)
    {
        Id = id;
        UserId = userId;
        Name = name;
        CurrencyId = currencyId;
    }
}
