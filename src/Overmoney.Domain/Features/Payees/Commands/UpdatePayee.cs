using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;

namespace Overmoney.Domain.Features.Payees.Commands;

public sealed record UpdatePayeeCommand(PayeeId Id, int UserId, string Name) : IRequest<Payee?>;

internal sealed class UpdatePayeeCommandValidator : AbstractValidator<UpdatePayeeCommand>
{
    public UpdatePayeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class UpdatePayeeCommandHandler : IRequestHandler<UpdatePayeeCommand, Payee?>
{
    private readonly IPayeeRepository _payeeRepository;

    public UpdatePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<Payee?> Handle(UpdatePayeeCommand request, CancellationToken cancellationToken)
    {
        var payee = await _payeeRepository.GetAsync(request.Id, cancellationToken);

        if (payee == null)
        {
            return await _payeeRepository.CreateAsync(new Payee(request.UserId, request.Name), cancellationToken);
        }

        await _payeeRepository.UpdateAsync(new Payee(request.Id, request.UserId, request.Name), cancellationToken);
        return null;
    }
}