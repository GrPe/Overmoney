using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Users;

namespace Overmoney.Api.Features.Users;

public sealed record RegisterUserCommand : IRequest<Response>
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
}

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
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

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email!, cancellationToken);
        if (user is not null)
        {
            return Response.FailureResponse("Cannot create new account for this email address");
        }

        await _userRepository.CreateAsync(new(request.UserName!, request.Email!, request.Password!), cancellationToken);

        _logger.LogInformation("Temporary account for email {email} created", request.Email);

        return Response.SuccessResponse;
    }
}
