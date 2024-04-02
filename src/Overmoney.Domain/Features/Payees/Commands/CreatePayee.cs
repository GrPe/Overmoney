using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Payees.Commands;

public sealed record CreatePayeeCommand(UserId UserId, string Name) : IRequest<Payee>;

internal sealed class CreatePayeeCommandValidator : AbstractValidator<CreatePayeeCommand>
{
    public CreatePayeeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class CreatePayeeCommandHandler : IRequestHandler<CreatePayeeCommand, Payee>
{
    private readonly IPayeeRepository _payeeRepository;

    public CreatePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<Payee> Handle(CreatePayeeCommand request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.CreateAsync(new Payee(request.UserId, request.Name), cancellationToken);
    }
}
