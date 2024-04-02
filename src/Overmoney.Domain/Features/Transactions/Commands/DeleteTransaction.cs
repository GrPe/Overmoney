using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record DeleteTransactionCommand(TransactionId Id) : IRequest;

internal sealed class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        await _transactionRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
