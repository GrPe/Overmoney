using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Commands;

namespace Overmoney.Api.Features.Transactions.Models;

public class UpdateTransaction
{
    public int Id { get; set; }
    public int WalletId { get; init; }
    public int UserId { get; init; }
    public int PayeeId { get; init; }
    public int CategoryId { get; init; }
    public DateTime TransactionDate { get; init; }
    public TransactionType TransactionType { get; init; }
    public string? Note { get; init; }
    public double Amount { get; init; }

    public UpdateTransaction(
        int id,
        int walletId,
        int userId,
        int payeeId,
        int categoryId,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount)
    {
        Id = id;
        WalletId = walletId;
        UserId = userId;
        PayeeId = payeeId;
        CategoryId = categoryId;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
    }

    public UpdateTransaction(int userId, UpdateTransactionCommand command)
    {
        Id = command.Id;
        WalletId = command.WalletId;
        UserId = userId;
        PayeeId = command.PayeeId;
        CategoryId = command.CategoryId;
        TransactionDate = command.TransactionDate;
        TransactionType = command.TransactionType;
        Note = command.Note;
        Amount = command.Amount;
    }
}
