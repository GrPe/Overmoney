using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(long Id) : IRequest<Transaction?>;

public sealed class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
{
    public GetTransactionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction?>
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
