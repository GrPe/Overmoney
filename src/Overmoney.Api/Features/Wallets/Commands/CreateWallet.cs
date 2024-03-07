using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Wallets.Commands;

public sealed record CreateWalletCommand(int UserId, string Name) : IRequest<int>;

public sealed class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, int>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateWalletCommandHandler> _logger;

    public CreateWalletCommandHandler(
        IWalletRepository walletRepository,
        IUserRepository userRepository,
        ILogger<CreateWalletCommandHandler> logger)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainValidationException($"User with id {request.UserId} doesn't exists");
        }

        var walletId = await _walletRepository.CreateAsync(new(request.UserId, request.Name), cancellationToken);

        _logger.LogInformation("Created new wallet for user {userId}", request.UserId);

        return walletId;
    }
}