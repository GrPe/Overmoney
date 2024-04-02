using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Queries;

public sealed record GetRecurringTransactionQuery(RecurringTransactionId Id) : IRequest<RecurringTransaction?>;

internal sealed class GetRecurringTransactionQueryValidator : AbstractValidator<GetRecurringTransactionQuery>
{
    public GetRecurringTransactionQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetRecurringTransactionQueryHandler : IRequestHandler<GetRecurringTransactionQuery, RecurringTransaction?>
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
