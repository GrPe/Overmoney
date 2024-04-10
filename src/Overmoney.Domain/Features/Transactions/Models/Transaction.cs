using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Transactions.Models;

[JsonConverter(typeof(LongIdentityJsonConverter))]
public sealed class TransactionId : Identity<long>
{
    public TransactionId(long id) : base(id)
    {
    }
}

public class Transaction
{
    public TransactionId? Id { get; }
    public UserId UserId { get; } = null!;
    public Wallet Wallet { get; }
    public Payee Payee { get; }
    public Category Category { get; }
    public DateTime TransactionDate { get; }
    public TransactionType TransactionType { get; }
    public string? Note { get; }
    public decimal Amount { get; }
    public List<Attachment> Attachments { get; }

    public Transaction(
        TransactionId id,
        UserId userId,
        Wallet wallet,
        Payee payee,
        Category category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        decimal amount,
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
        UserId userId,
        Wallet wallet,
        Payee payee,
        Category category,
        DateTime transactionDate,
        TransactionType transactionType,
        string? note,
        decimal amount,
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