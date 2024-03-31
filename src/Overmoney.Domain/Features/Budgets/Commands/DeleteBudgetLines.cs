using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Budgets.Models;

namespace Overmoney.Domain.Features.Budgets.Commands;

public sealed record DeleteBudgetLinesCommand(BudgetId BudgetId, IEnumerable<int> BudgetLines) : IRequest;

internal sealed class DeleteBudgetLinesCommandValidator : AbstractValidator<DeleteBudgetLinesCommand>
{
    public DeleteBudgetLinesCommandValidator()
    {
        RuleFor(x => x.BudgetId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });

        RuleForEach(x => x.BudgetLines)
            .GreaterThan(0);
    }
}

internal sealed class DeleteBudgetLinesCommandHandler : IRequestHandler<DeleteBudgetLinesCommand>
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