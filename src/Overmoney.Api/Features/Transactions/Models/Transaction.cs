using Overmoney.Api.Features.Categories.Models;
using Overmoney.Api.Features.Payees.Models;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.Features.Transactions.Models;

public class Transaction
{
    public long Id { get; }
    public int UserId { get; }
    public Wallet Wallet { get; }
    public Payee Payee { get; }
    public Category Category { get; }
    public DateTime TransactionDate { get; }
    public TransactionType TransactionType { get; }
    public string? Note { get; }
    public double Amount { get; }
    public List<Attachment> Attachments { get; }

    public Transaction(
        long id,
        int userId,
        Wallet wallet,
        Payee payee,
        Category category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
        List<Attachment>? attachments = null)
    {
        Id = id;
        UserId = userId;
        Wallet = wallet;
        Payee = payee;
        Category = category;
        TransactionDate = transactionDate;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Attachments = attachments ?? [];
    }

    public Transaction(
        int userId,
        Wallet wallet,
        Payee payee,
        Category category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        double amount,
    List<Attachment>? attachments = null)
    {
        UserId = userId;
        Wallet = wallet;
        Payee = payee;
        Category = category;
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
