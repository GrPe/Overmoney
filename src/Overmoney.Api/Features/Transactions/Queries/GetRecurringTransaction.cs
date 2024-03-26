using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions.Queries;

public sealed record GetRecurringTransactionQuery(long Id) : IRequest<RecurringTransaction?>;

public sealed class GetRecurringTransactionQueryValidator : AbstractValidator<GetRecurringTransactionQuery>
{
    public GetRecurringTransactionQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetRecurringTransactionQueryHandler : IRequestHandler<GetRecurringTransactionQuery, RecurringTransaction?>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetRecurringTransactionQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<RecurringTransaction?> Handle(GetRecurringTransactionQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetRecurringTransactionAsync(request.Id, cancellationToken);
    }
}
