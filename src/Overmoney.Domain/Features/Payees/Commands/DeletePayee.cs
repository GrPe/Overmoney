using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;

namespace Overmoney.Domain.Features.Payees.Commands;

public sealed record DeletePayeeCommand(PayeeId Id) : IRequest;

internal sealed class DeletePayeeCommandValidator : AbstractValidator<DeletePayeeCommand>
{
    public DeletePayeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal sealed class DeletePayeeCommandHandler : IRequestHandler<DeletePayeeCommand>
{
    private readonly IPayeeRepository _payeeRepository;

    public DeletePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task Handle(DeletePayeeCommand request, CancellationToken cancellationToken)
    {
        await _payeeRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
