using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Budgets.Models;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.Domain.Features.Budgets.Commands;

public sealed record UpsertBudgetLineCommand(BudgetId BudgetId, IEnumerable<UpsertBudgetLine> BudgetLines) : IRequest;
public sealed record UpsertBudgetLine(BudgetLineId? BudgetLineId, CategoryId CategoryId, decimal Amount);

internal sealed class UpsertBudgetLineCommandValidator : AbstractValidator<UpsertBudgetLineCommand>
{
    public UpsertBudgetLineCommandValidator()
    {
        RuleFor(x => x.BudgetId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.BudgetLines)
            .NotEmpty();
        RuleForEach(x => x.BudgetLines)
            .ChildRules(x =>
            {
                x.RuleFor(x => x.CategoryId)
                    .NotEmpty()
                    .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
            });
    }
}

internal sealed class UpsertBudgetLineCommandHandler : IRequestHandler<UpsertBudgetLineCommand>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpsertBudgetLineCommandHandler(
        IBudgetRepository budgetRepository,
        ICategoryRepository categoryRepository)
    {
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpsertBudgetLineCommand request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetAsync(request.BudgetId, cancellationToken);

        if (budget is null)
        {
            throw new DomainValidationException("Budget doesn't exists");
        }

        var categories = await _categoryRepository.GetAllByUserAsync(budget.UserId, cancellationToken);

        foreach (var requestBudgetLine in request.BudgetLines)
        {
            if (requestBudgetLine.BudgetLineId is not null && requestBudgetLine.Amount == 0)
            {
                budget.RemoveBudgetLine(requestBudgetLine.BudgetLineId);
                continue;
            }

            var category = categories.FirstOrDefault(x => x.Id == requestBudgetLine.CategoryId);
            if (category is null)
            {
                throw new DomainValidationException($"Category with id: {requestBudgetLine.CategoryId} doesn't exists");
            }
            budget.UpsertBudgetLine(new BudgetLine(requestBudgetLine.BudgetLineId, category, requestBudgetLine.Amount));
        }

        await _budgetRepository.UpdateAsync(budget, cancellationToken);
    }
}