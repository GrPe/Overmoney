using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Wallets.Commands;

public sealed record DeleteWalletCommand(WalletId Id) : IRequest;

internal sealed class DeleteWalletCommandValidator : AbstractValidator<DeleteWalletCommand>
{
    public DeleteWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand>
{
    private readonly IWalletRepository _walletRepository;

    public DeleteWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        await _walletRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
