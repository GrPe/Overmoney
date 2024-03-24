using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record UpdateRecurringTransactionCommand(
    long Id,
    int WalletId,
    int PayeeId,
    int CategoryId,
    DateTime TransactionDate,
    TransactionType TransactionType,
    string? Note,
    double Amount,
    string Schedule) : IRequest<RecurringTransaction?>;

public sealed class UpdateRecurringTransactionCommandValidator : AbstractValidator<UpdateRecurringTransactionCommand>
{
    public UpdateRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.WalletId)
            .GreaterThan(0);
        RuleFor(x => x.PayeeId)
            .GreaterThan(0);
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
        RuleFor(x => x.TransactionDate)
            .NotEmpty();
        RuleFor(x => x.Schedule)
            .NotEmpty();
    }
}

public sealed class UpdateRecurringTransactionCommandHandler : IRequestHandler<UpdateRecurringTransactionCommand, RecurringTransaction?>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPayeeRepository _payeeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMediator _mediator;

    public UpdateRecurringTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IPayeeRepository payeeRepository,
        ICategoryRepository categoryRepository,
        IMediator mediator)
    {
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _payeeRepository = payeeRepository;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RecurringTransaction?> Handle(UpdateRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetRecurringTransactionAsync(request.Id, cancellationToken);

        if (transaction is null)
        {
            return await _mediator.Send(new CreateRecurringTransactionCommand(request.WalletId, request.PayeeId, request.CategoryId, request.TransactionDate, request.TransactionType, request.Note, request.Amount, request.Schedule), cancellationToken);
        }

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

        await _transactionRepository.UpdateAsync(new RecurringTransaction(transaction.Id, wallet.UserId, wallet, payee, category, request.TransactionDate, request.TransactionType, request.Note, request.Amount, new Schedule(request.Schedule)), cancellationToken);
        return null;
    }
}