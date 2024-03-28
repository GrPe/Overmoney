using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.Features.Payees.Commands;

public sealed record DeletePayeeCommand(PayeeId Id) : IRequest;

public sealed class DeletePayeeCommandValidator : AbstractValidator<DeletePayeeCommand>
{
    public DeletePayeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public sealed class DeletePayeeCommandHandler : IRequestHandler<DeletePayeeCommand>
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
