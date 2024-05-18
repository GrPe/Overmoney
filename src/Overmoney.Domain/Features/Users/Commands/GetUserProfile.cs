using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Users.Commands;

public sealed record GetUserProfile(string? Email) : IRequest<UserProfile?> { }

internal sealed class GetUserProfileCommandValidator : AbstractValidator<GetUserProfile>
{
    public GetUserProfileCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
    }
}

internal sealed class GetUserProfileCommandHandler : IRequestHandler<GetUserProfile, UserProfile?>
{
    private readonly IUserProfileRepository _userRepository;

    public GetUserProfileCommandHandler(IUserProfileRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfile?> Handle(GetUserProfile request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email!, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return user;
    }
}