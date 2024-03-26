using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Budgets;

namespace Overmoney.Api.Features.Budgets.Commands;

public sealed record DeleteBudgetCommand(int Id) : IRequest;

public sealed class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
{
    public DeleteBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand>
{
    private readonly IBudgetRepository _budgetRepository;

    public DeleteBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        await _budgetRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
