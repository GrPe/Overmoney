namespace Overmoney.Api.Features.Transactions.Models;

public class Transaction
{
    public long Id { get; }
    public int WalletId { get; }
    public int UserId { get; }
    public int PayeeId { get; }
    public int CategoryId { get; }
    public DateTime TransactionDate { get; }
    public TransactionType TransactionType { get; }
    public string? Note { get; }
    public double Amount { get; }
    public List<Attachment> Attachments { get; }

    public Transaction(
        long id,
        int walletId,
        int userId,
        int payeeId,
        int categoryId,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
        List<Attachment>? attachments = null)
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
        Attachments = attachments ?? [];
    }

    public Transaction(
        int walletId,
        int userId,
        int payeeId,
        int categoryId,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
        List<Attachment>? attachments = null)
    {
        WalletId = walletId;
        UserId = userId;
        PayeeId = payeeId;
        CategoryId = categoryId;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Attachments = attachments ?? [];
    }

    public void AddAttachment(Attachment attachment)
    {
        Attachments.Add(attachment);
    }
}

public enum TransactionType
{
    Outcome = 0,
    Income = 1,
    Transfer = 2
}
