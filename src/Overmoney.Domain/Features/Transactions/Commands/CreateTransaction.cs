using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(
    WalletId WalletId,
    PayeeId PayeeId,
    CategoryId CategoryId,
    DateOnly TransactionDate,
    TransactionType TransactionType,
    string? Note,
    decimal Amount,
    TransactionAttachment[]? Attachments) : IRequest<Transaction>;

public sealed record TransactionAttachment(string Name, string Path);

internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.PayeeId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.TransactionDate)
            .NotEmpty();
        RuleForEach(x => x.Attachments).ChildRules(a =>
        {
            a.RuleFor(x => x.Name).NotEmpty();
            a.RuleFor(x => x.Path).NotEmpty();
        });
    }
}

internal sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Transaction>
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

        return await _transactionRepository.CreateAsync(new Transaction(wallet.UserId, wallet, payee, category, request.TransactionDate, request.TransactionType, request.Note, request.Amount, request.Attachments?.Select(x => new Attachment(x.Name, x.Path)).ToList()), cancellationToken);
    }
}