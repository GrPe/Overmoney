using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;

namespace Overmoney.Domain.Features.Payees.Commands;

public sealed record CreatePayeeCommand(int UserId, string Name) : IRequest<Payee>;

internal sealed class CreatePayeeCommandValidator : AbstractValidator<CreatePayeeCommand>
{
    public CreatePayeeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
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
