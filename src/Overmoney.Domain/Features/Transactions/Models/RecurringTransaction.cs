﻿using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Transactions.Models;

[JsonConverter(typeof(LongIdentityJsonConverter))]
public sealed class RecurringTransactionId : Identity<long>
{
    public RecurringTransactionId(long id) : base(id)
    { }
}

public class RecurringTransaction
{
    public RecurringTransactionId? Id { get; }
    public UserId UserId { get; } = null!;
    public Wallet Wallet { get; }
    public Payee Payee { get; }
    public Category Category { get; }
    public TransactionType TransactionType { get; }
    public string? Note { get; }
    public double Amount { get; }
    public Schedule Schedule { get; }
    public DateTime NextOccurrence { get; private set; }


    public RecurringTransaction(
        RecurringTransactionId id,
        UserId userId,
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
        UserId userId,
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