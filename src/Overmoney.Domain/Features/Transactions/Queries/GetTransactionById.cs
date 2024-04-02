using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(TransactionId Id) : IRequest<Transaction?>;

internal sealed class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
{
    public GetTransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction?>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Transaction?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetAsync(request.Id, cancellationToken);
    }
}
