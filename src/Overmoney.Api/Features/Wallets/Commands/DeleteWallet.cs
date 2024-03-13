using FluentValidation;
using MediatR;
using Overmoney.Api.Features.Wallets;

namespace Overmoney.Api.Features.Wallets.Commands;

public sealed record DeleteWalletCommand(int Id) : IRequest;

public sealed class DeleteWalletCommandValidator : AbstractValidator<DeleteWalletCommand>
{
    public DeleteWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand>
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
