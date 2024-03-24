using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record DeleteRecurringTransactionCommand(long Id) : IRequest;

public sealed class DeleteRecurringTransactionCommandValidator : AbstractValidator<DeleteRecurringTransactionCommand>
{
    public DeleteRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeleteRecurringTransactionCommandHandler : IRequestHandler<DeleteRecurringTransactionCommand>
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