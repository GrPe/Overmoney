using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;

namespace Overmoney.Api.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(int Id) : IRequest<TransactionEntity?>;

public sealed class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
{
    public GetTransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionEntity?>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionEntity?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetAsync(request.Id, cancellationToken);
    }
}
