﻿using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record UpdateRecurringTransactionCommand(
    RecurringTransactionId Id,
    WalletId WalletId,
    PayeeId PayeeId,
    CategoryId CategoryId,
    TransactionType TransactionType,
    string? Note,
    decimal Amount,
    DateTime FirstOccurrence,
    string Schedule) : IRequest<RecurringTransaction?>;

internal sealed class UpdateRecurringTransactionCommandValidator : AbstractValidator<UpdateRecurringTransactionCommand>
{
    public UpdateRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.PayeeId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
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

internal sealed class UpdateRecurringTransactionCommandHandler : IRequestHandler<UpdateRecurringTransactionCommand, RecurringTransaction?>
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
            return await _mediator.Send(new CreateRecurringTransactionCommand(request.WalletId, request.PayeeId, request.CategoryId, request.TransactionType, request.Note, request.Amount, request.FirstOccurrence, request.Schedule), cancellationToken);
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

        await _transactionRepository.UpdateAsync(new RecurringTransaction(transaction.Id!, wallet.UserId, wallet, payee, category, request.TransactionType, request.Note, request.Amount, new Schedule(request.Schedule), request.FirstOccurrence), cancellationToken);
        return null;
    }
}