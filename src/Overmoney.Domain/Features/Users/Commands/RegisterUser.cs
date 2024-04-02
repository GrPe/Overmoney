using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Users.Commands;

public sealed record RegisterUserCommand : IRequest<UserId>
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
}

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserId>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserId> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email!, cancellationToken);
        if (user is not null)
        {
            throw new DomainValidationException("Cannot create new account for this email address");
        }

        var userId = await _userRepository.CreateAsync(new(request.UserName!, request.Email!, request.Password!), cancellationToken);

        _logger.LogInformation("Temporary account for email {email} created", request.Email);

        return userId;
    }
}
