using FluentValidation;
using MediatR;
using Overmoney.Domain;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record UpdateRecurringTransactionNextOccurrenceCommand(RecurringTransactionId Id) : IRequest;

internal sealed class UpdateRecurringTransactionNextOccurrenceCommandValidator : AbstractValidator<UpdateRecurringTransactionNextOccurrenceCommand>
{
    public UpdateRecurringTransactionNextOccurrenceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class UpdateRecurringTransactionNextOccurrenceCommandHandler : IRequestHandler<UpdateRecurringTransactionNextOccurrenceCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly TimeProvider _dateTimeProvider;

    public UpdateRecurringTransactionNextOccurrenceCommandHandler(
        ITransactionRepository transactionRepository,
        TimeProvider dateTimeProvider)
    {
        _transactionRepository = transactionRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(UpdateRecurringTransactionNextOccurrenceCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetRecurringTransactionAsync(request.Id, cancellationToken);

        if (transaction is null)
        {
            throw new DomainValidationException("Recurring transaction not found");
        }

        transaction.UpdateSchedule(_dateTimeProvider.GetUtcNow().DateTime);
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
    }
}
