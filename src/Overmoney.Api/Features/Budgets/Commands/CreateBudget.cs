using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Budgets;
using Overmoney.Api.Features.Budgets.Models;

namespace Overmoney.Api.Features.Budgets.Commands;

public sealed record CreateBudgetCommand(int UserId, string Name, int Year, int Month) : IRequest<Budget>;

public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Year)
            .InclusiveBetween(1970, 2100);
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}

public sealed class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Budget>
{
    private readonly IBudgetRepository _budgetRepository;

    public CreateBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<Budget> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        return await _budgetRepository.CreateAsync(new Budget(request.UserId, request.Name, request.Year, request.Month), cancellationToken);
    }
}