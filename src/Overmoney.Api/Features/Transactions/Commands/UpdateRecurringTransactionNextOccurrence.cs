using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Infrastructure;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record UpdateRecurringTransactionNextOccurrenceCommand(long Id) : IRequest;

public sealed class UpdateRecurringTransactionNextOccurrenceCommandValidator : AbstractValidator<UpdateRecurringTransactionNextOccurrenceCommand>
{
    public UpdateRecurringTransactionNextOccurrenceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class UpdateRecurringTransactionNextOccurrenceCommandHandler : IRequestHandler<UpdateRecurringTransactionNextOccurrenceCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateRecurringTransactionNextOccurrenceCommandHandler(
        ITransactionRepository transactionRepository, 
        IDateTimeProvider dateTimeProvider)
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

        transaction.UpdateSchedule(_dateTimeProvider.UtcNow);
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
    }
}
