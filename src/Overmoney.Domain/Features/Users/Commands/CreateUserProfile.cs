using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Users.Commands;

public sealed record CreateUserProfileCommand(string? Email) : IRequest<UserProfile>
{ }

internal sealed class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
{
    public CreateUserProfileCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}

internal class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfile>
{
    private readonly IUserProfileRepository _userRepository;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;

    public CreateUserProfileCommandHandler(IUserProfileRepository userRepository, ILogger<CreateUserProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserProfile> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email!, cancellationToken);

        if (user is not null)
        {
            throw new DomainValidationException("Cannot create new account for this email address");
        }

        var userProfile = await _userRepository.CreateAsync(new(request.Email!), cancellationToken);

        _logger.LogInformation("Temporary account for email {email} created", request.Email);

        return userProfile;
    }
}
