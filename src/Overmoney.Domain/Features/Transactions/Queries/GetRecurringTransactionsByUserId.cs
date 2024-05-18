using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Transactions.Queries;

public sealed record GetRecurringTransactionsByUserIdQuery(UserProfileId UserId) : IRequest<IEnumerable<RecurringTransaction>>;

internal sealed class GetRecurringTransactionsByUserIdQueryValidator : AbstractValidator<GetRecurringTransactionsByUserIdQuery>
{
    public GetRecurringTransactionsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetRecurringTransactionsByUserIdQueryHandler : IRequestHandler<GetRecurringTransactionsByUserIdQuery, IEnumerable<RecurringTransaction>>
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