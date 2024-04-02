using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Wallets.Commands;

public sealed record UpdateWalletCommand(WalletId Id, UserId UserId, string Name, int CurrencyId) : IRequest<Wallet?>;

internal sealed class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, Wallet?>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public UpdateWalletCommandHandler(
        IWalletRepository walletRepository,
        IUserRepository userRepository,
        ICurrencyRepository currencyRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
    }

    public async Task<Wallet?> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainValidationException($"User with id {request.UserId} doesn't exists");
        }

        var currency = await _currencyRepository.GetAsync(request.CurrencyId, cancellationToken);

        if (currency is null)
        {
            throw new DomainValidationException($"Currency with id {request.CurrencyId} doesn't exists.");
        }

        var wallet = await _walletRepository.GetAsync(request.Id, cancellationToken);

        if (wallet is null)
        {
            return await _walletRepository.CreateAsync(new(request.Name, currency, user.Id), cancellationToken);
        }

        await _walletRepository.UpdateAsync(new Wallet(request.Id, request.Name, currency, user.Id), cancellationToken);
        return null;
    }
}
