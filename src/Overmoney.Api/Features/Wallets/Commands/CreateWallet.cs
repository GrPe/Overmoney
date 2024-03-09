using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.DataAccess.Wallets.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Wallets.Commands;

public sealed record CreateWalletCommand(int UserId, string Name, int CurrencyId) : IRequest<Wallet>;

public sealed class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Wallet>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ILogger<CreateWalletCommandHandler> _logger;

    public CreateWalletCommandHandler(
        IWalletRepository walletRepository,
        IUserRepository userRepository,
        ILogger<CreateWalletCommandHandler> logger,
        ICurrencyRepository currencyRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _logger = logger;
        _currencyRepository = currencyRepository;
    }

    public async Task<Wallet> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
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

        var wallet = await _walletRepository.CreateAsync(new(request.UserId, request.Name, request.CurrencyId), cancellationToken);

        _logger.LogInformation("Created new wallet for user {userId}", request.UserId);

        return wallet;
    }
}