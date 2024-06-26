﻿using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Wallets.Queries;

public sealed record GetWalletQuery(WalletId Id) : IRequest<Wallet?>;

internal sealed class GetWalletQueryValidator : AbstractValidator<GetWalletQuery>
{
    public GetWalletQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, Wallet?>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Wallet?> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        return await _walletRepository.GetAsync(request.Id, cancellationToken);
    }
}
