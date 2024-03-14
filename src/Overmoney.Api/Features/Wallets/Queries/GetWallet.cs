using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.Features.Wallets.Queries;

public sealed record GetWalletQuery(int Id) : IRequest<WalletEntity?>;

public sealed class GetWalletQueryValidator : AbstractValidator<GetWalletQuery>
{
    public GetWalletQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, WalletEntity?>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<WalletEntity?> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        return await _walletRepository.GetAsync(request.Id, cancellationToken);
    }
}
