using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record DeleteRecurringTransactionCommand(RecurringTransactionId Id) : IRequest;

internal sealed class DeleteRecurringTransactionCommandValidator : AbstractValidator<DeleteRecurringTransactionCommand>
{
    public DeleteRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class DeleteRecurringTransactionCommandHandler : IRequestHandler<DeleteRecurringTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public DeleteRecurringTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(DeleteRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        await _transactionRepository.DeleteRecurringTransactionAsync(request.Id, cancellationToken);
    }
}