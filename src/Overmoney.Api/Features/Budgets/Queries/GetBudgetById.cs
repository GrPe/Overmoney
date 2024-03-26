using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Budgets;
using Overmoney.Api.Features.Budgets.Models;

namespace Overmoney.Api.Features.Budgets.Queries;

public sealed record GetBudgetByIdQuery(int Id) : IRequest<Budget?>;

public sealed class GetBudgetByIdQueryValidator : AbstractValidator<GetBudgetByIdQuery>
{
    public GetBudgetByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, Budget?>
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
