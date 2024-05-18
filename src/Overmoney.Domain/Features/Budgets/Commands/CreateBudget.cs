using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Budgets.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Budgets.Commands;

public sealed record CreateBudgetCommand(UserProfileId UserId, string Name, int Year, int Month) : IRequest<Budget>;

internal sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.UserId)
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

internal sealed class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Budget>
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