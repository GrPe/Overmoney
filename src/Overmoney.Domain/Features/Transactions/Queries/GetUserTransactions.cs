using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Transactions.Queries;
public record GetUserTransactionsQuery(UserProfileId UserId, WalletId? WalletId, CategoryId? CategoryId, PayeeId? PayeeId) : IRequest<IEnumerable<Transaction>>;

public class GetUserTransactionsQueryValidator : AbstractValidator<GetUserTransactionsQuery>
{
    public GetUserTransactionsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal class GetUserTransactionsQueryHandler : IRequestHandler<GetUserTransactionsQuery, IEnumerable<Transaction>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetUserTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<Transaction>> Handle(GetUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetUserTransactionsAsync(request.UserId, request.WalletId, request.CategoryId, request.PayeeId, cancellationToken);
    }
}
