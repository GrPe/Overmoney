using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Users.Commands;

public sealed record DeleteUserCommand(UserId UserId) : IRequest { }

internal sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteByIdAsync(request.UserId, cancellationToken);
        _logger.LogInformation("User with id: {id} deleted", request.UserId);
    }
}
