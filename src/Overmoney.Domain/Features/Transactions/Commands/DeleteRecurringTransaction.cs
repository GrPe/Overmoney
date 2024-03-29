using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record DeleteRecurringTransactionCommand(long Id) : IRequest;

internal sealed class DeleteRecurringTransactionCommandValidator : AbstractValidator<DeleteRecurringTransactionCommand>
{
    public DeleteRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
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