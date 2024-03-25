using Overmoney.Api.Features.Categories.Models;
using Overmoney.Api.Features.Payees.Models;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.Features.Transactions.Models;

public class RecurringTransaction
{
    public long Id { get; }
    public int UserId { get; }
    public Wallet Wallet { get; }
    public Payee Payee { get; }
    public Category Category { get; }
    public TransactionType TransactionType { get; }
    public string? Note { get; }
    public double Amount { get; }
    public Schedule Schedule { get; }
    public DateTime NextOccurrence { get; private set; }


    public RecurringTransaction(
        long id,
        int userId,
        Wallet wallet,
        Payee payee,
        Category category,
        TransactionType transactionType,
        string? note,
        double amount,
        Schedule schedule,
        DateTime nextOccurrence)
    {
        Id = id;
        UserId = userId;
        Wallet = wallet;
        Payee = payee;
        Category = category;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Schedule = schedule;
        NextOccurrence = nextOccurrence;
    }

    public RecurringTransaction(
        int userId,
        Wallet wallet,
        Payee payee,
        Category category,
        TransactionType transactionType,
        string? note,
        double amount,
        Schedule schedule,
        DateTime nextOccurrence)
    {
        UserId = userId;
        Wallet = wallet;
        Payee = payee;
        Category = category;
        TransactionType = transactionType;
        Note = note;
        Amount = amount;
        Schedule = schedule;
        NextOccurrence = nextOccurrence;
    }

    public void UpdateSchedule(DateTime currentDate)
    {
        NextOccurrence = Schedule.NextOccurrence(currentDate);
    }
}
