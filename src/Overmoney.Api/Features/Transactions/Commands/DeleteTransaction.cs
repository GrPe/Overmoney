using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record DeleteTransactionCommand(int Id) : IRequest;

public sealed class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
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
