using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record CreateRecurringTransactionCommand(
    int WalletId,
    int PayeeId,
    int CategoryId,
    TransactionType TransactionType,
    string? Note,
    double Amount,
    DateTime FirstOccurrence,
    string Schedule) : IRequest<RecurringTransaction>;

public sealed class CreateRecurringTransactionCommandValidator : AbstractValidator<CreateRecurringTransactionCommand>
{
    public CreateRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .GreaterThan(0);
        RuleFor(x => x.PayeeId)
            .GreaterThan(0);
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
        RuleFor(x => x.FirstOccurrence)
            .NotEmpty();
        RuleFor(x => x.Schedule)
            .NotEmpty()
            .Custom((schedule, context) =>
            {
                if (!Cronos.CronExpression.TryParse(schedule, out var _))
                {
                    context.AddFailure("Invalid schedule format. Use (* * * * *)");
                }
            });
    }
}

public sealed class CreateRecurringTransactionCommandHandler : IRequestHandler<CreateRecurringTransactionCommand, RecurringTransaction>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPayeeRepository _payeeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CreateRecurringTransactionCommandHandler(
        IWalletRepository walletRepository,
        IPayeeRepository payeeRepository,
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository)
    {
        _walletRepository = walletRepository;
        _payeeRepository = payeeRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<RecurringTransaction> Handle(CreateRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAsync(request.WalletId, cancellationToken);

        if (wallet is null)
        {
            throw new DomainValidationException($"Wallet of id {request.WalletId} does not exists.");
        }

        var category = await _categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new DomainValidationException($"Category of id {request.CategoryId} does not exists.");
        }

        var payee = await _payeeRepository.GetAsync(request.PayeeId, cancellationToken);

        if (payee is null)
        {
            throw new DomainValidationException($"Payee of id {request.PayeeId} does not exists.");
        }

        return await _transactionRepository.CreateAsync(new RecurringTransaction(wallet.UserId, wallet, payee, category, request.TransactionType, request.Note, request.Amount, new Schedule(request.Schedule), request.FirstOccurrence), cancellationToken);
    }
}