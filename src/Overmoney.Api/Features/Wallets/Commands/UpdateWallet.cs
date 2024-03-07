using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Users;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.DataAccess.Wallets.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Wallets.Commands;

public sealed record UpdateWalletCommand(int Id, int UserId, string Name) : IRequest<int?>;

public sealed class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, int?>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;

    public UpdateWalletCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
    }

    public async Task<int?> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new DomainValidationException($"User with id {request.UserId} doesn't exists");
        }

        var wallet = await _walletRepository.GetAsync(request.Id, cancellationToken);

        if (wallet is null)
        {
            var walletId = await _walletRepository.CreateAsync(new(user.Id, request.Name), cancellationToken);
            return walletId;
        }

        await _walletRepository.UpdateAsync(new UpdateWallet(request.Id, request.Name), cancellationToken);
        return null;
    }
}
