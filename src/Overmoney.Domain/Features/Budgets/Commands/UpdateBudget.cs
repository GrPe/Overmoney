using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Budgets.Models;

namespace Overmoney.Domain.Features.Budgets.Commands;

public sealed record UpdateBudgetCommand(BudgetId Id, string Name, int Year, int Month) : IRequest;

internal sealed class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Year)
            .InclusiveBetween(1970, 2100);
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}

internal sealed class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand>
{
    private readonly IBudgetRepository _budgetRepository;

    public UpdateBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetAsync(request.Id, cancellationToken);

        if (budget is null)
        {
            throw new DomainValidationException("Budget doesn't exists");
        }

        budget.Update(request.Name, request.Year, request.Month);

        await _budgetRepository.UpdateAsync(budget, cancellationToken);
    }
}
