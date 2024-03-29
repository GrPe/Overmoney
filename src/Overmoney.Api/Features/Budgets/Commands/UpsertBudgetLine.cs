﻿using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Budgets;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.Features.Budgets.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Budgets.Commands;

public sealed record UpsertBudgetLineCommand(int BudgetId, IEnumerable<UpsertBudgetLine> BudgetLines) : IRequest;
public sealed record UpsertBudgetLine(long? BudgetLineId, int CategoryId, double Amount);

public sealed class UpsertBudgetLineCommandValidator : AbstractValidator<UpsertBudgetLineCommand>
{
    public UpsertBudgetLineCommandValidator()
    {
        RuleFor(x => x.BudgetId)
            .GreaterThan(0);
        RuleFor(x => x.BudgetLines)
            .NotEmpty();
        RuleForEach(x => x.BudgetLines)
            .ChildRules(x =>
            {
                x.RuleFor(x => x.CategoryId)
                    .GreaterThan(0);
            });
    }
}

public sealed class UpsertBudgetLineCommandHandler : IRequestHandler<UpsertBudgetLineCommand>
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
                budget.RemoveBudgetLine(requestBudgetLine.BudgetLineId.Value);
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