using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Budgets.Models;

namespace Overmoney.Domain.Features.Budgets.Queries;

public sealed record GetBudgetByIdQuery(int Id) : IRequest<Budget?>;

internal sealed class GetBudgetByIdQueryValidator : AbstractValidator<GetBudgetByIdQuery>
{
    public GetBudgetByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

internal sealed class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, Budget?>
{
    private readonly IBudgetRepository _budgetRepository;

    public GetBudgetByIdQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<Budget?> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _budgetRepository.GetAsync(request.Id, cancellationToken);
    }
}
