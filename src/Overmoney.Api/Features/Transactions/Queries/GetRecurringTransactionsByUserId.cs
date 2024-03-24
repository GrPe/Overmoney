using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions.Queries;

public sealed record GetRecurringTransactionsByUserIdQuery(int UserId) : IRequest<IEnumerable<RecurringTransaction>>;

public sealed class GetRecurringTransactionsByUserIdQueryValidator : AbstractValidator<GetRecurringTransactionsByUserIdQuery>
{
    public GetRecurringTransactionsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}

public sealed class GetRecurringTransactionsByUserIdQueryHandler : IRequestHandler<GetRecurringTransactionsByUserIdQuery, IEnumerable<RecurringTransaction>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetRecurringTransactionsByUserIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<RecurringTransaction>> Handle(GetRecurringTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetRecurringTransactionsByUserIdAsync(request.UserId, cancellationToken);
    }
}