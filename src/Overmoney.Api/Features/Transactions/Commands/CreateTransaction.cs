using FluentValidation;
using MediatR;
using Overmoney.Api.Infrastructure.Exceptions;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.DataAccess.Wallets;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(
    int WalletId,
    int PayeeId,
    int CategoryId,
    DateTime TransactionDate,
    TransactionType TransactionType,
    string? Note,
    double Amount) : IRequest<Transaction>;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .GreaterThan(0);
        RuleFor(x => x.PayeeId)
            .GreaterThan(0);
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
        RuleFor(x => x.TransactionDate)
            .NotEmpty();
    }
}

public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Transaction>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPayeeRepository _payeeRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandler(
        IWalletRepository walletRepository, 
        ICategoryRepository categoryRepository, 
        IPayeeRepository payeeRepository, 
        ITransactionRepository transactionRepository)
    {
        _walletRepository = walletRepository;
        _categoryRepository = categoryRepository;
        _payeeRepository = payeeRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
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

        return await _transactionRepository.CreateAsync(new Transaction(wallet.Id, wallet.UserId, request.PayeeId, request.CategoryId, request.TransactionDate, request.TransactionType, request.Note, request.Amount), cancellationToken);
    }
}