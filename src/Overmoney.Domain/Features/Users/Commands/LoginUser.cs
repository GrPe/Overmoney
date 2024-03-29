using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Overmoney.Domain.DataAccess;

namespace Overmoney.Domain.Features.Users.Commands;

public sealed record LoginUserCommand(string? Login, string? Password) : IRequest<int?> { }

internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, int?>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(IUserRepository userRepository, ILogger<LoginUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<int?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (user is null)
        {
            return null;
        }

        //validate password

        return user.Id;
    }
}