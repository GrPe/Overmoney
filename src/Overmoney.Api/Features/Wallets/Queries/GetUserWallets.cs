using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.Features.Wallets.Queries;

public sealed record GetUserWalletsQuery(int UserId) : IRequest<IEnumerable<WalletEntity>> { }

public sealed class GetUserWalletsQueryValidator : AbstractValidator<GetUserWalletsQuery>
{
    public GetUserWalletsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}

public sealed class GetUserWalletsQueryHandler : IRequestHandler<GetUserWalletsQuery, IEnumerable<WalletEntity>>
{
    private readonly IWalletRepository _walletRepository;

    public GetUserWalletsQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<IEnumerable<WalletEntity>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
    {
        var wallets = await _walletRepository.GetByUserAsync(request.UserId, cancellationToken);
        return wallets;
    }
}

