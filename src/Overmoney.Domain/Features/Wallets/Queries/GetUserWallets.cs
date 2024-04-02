using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Wallets.Queries;

public sealed record GetUserWalletsQuery(UserId UserId) : IRequest<IEnumerable<Wallet>> { }

internal sealed class GetUserWalletsQueryValidator : AbstractValidator<GetUserWalletsQuery>
{
    public GetUserWalletsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetUserWalletsQueryHandler : IRequestHandler<GetUserWalletsQuery, IEnumerable<Wallet>>
{
    private readonly IWalletRepository _walletRepository;

    public GetUserWalletsQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<IEnumerable<Wallet>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
    {
        var wallets = await _walletRepository.GetByUserAsync(request.UserId, cancellationToken);
        return wallets;
    }
}

