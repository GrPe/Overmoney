namespace Overmoney.Api.DataAccess.Transactions;

internal class TransactionEntity
{
    public long Id { get; init; }
    public int WalletId { get; init; }
    public int UserId { get; init; }
    public int PayeeId { get; init; }
    public int CategoryId { get; init; }
    public DateTime TransactionDate { get; init; }
    public TransactionType TransactionType { get; init; }
    public string? Note { get; init; }
    public double Amount { get; init; }

    public TransactionEntity(
        long id,
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
}

public enum TransactionType
{
    Outcome = 0,
    Income = 1,
    Transfer = 2
}
