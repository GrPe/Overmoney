using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Budgets;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Budgets.Commands;

public sealed record DeleteBudgetLinesCommand(int BudgetId, IEnumerable<int> BudgetLines) : IRequest;

public sealed class DeleteBudgetLinesCommandValidator : AbstractValidator<DeleteBudgetLinesCommand>
{
    public DeleteBudgetLinesCommandValidator()
    {
        RuleFor(x => x.BudgetId)
            .GreaterThan(0);

        RuleForEach(x => x.BudgetLines)
            .GreaterThan(0);
    }
}

public sealed class DeleteBudgetLinesCommandHandler : IRequestHandler<DeleteBudgetLinesCommand>
{
    private readonly IBudgetRepository _budgetRepository;

    public DeleteBudgetLinesCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task Handle(DeleteBudgetLinesCommand request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetAsync(request.BudgetId, cancellationToken);

        if (budget is null)
        {
            throw new DomainValidationException("Budget doesn't exists");
        }

        foreach (var budgetLineId in request.BudgetLines)
        {
            budget.RemoveBudgetLine(budgetLineId);
        }

        await _budgetRepository.UpdateAsync(budget, cancellationToken);
    }
}