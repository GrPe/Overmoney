namespace Overmoney.Api.DataAccess.Transactions;

public class Transaction
{
    public long Id { get; init; }
    public int AccountId { get; init; }
    public int UserId { get; init; }
    public int PayeeId {  get; init; }
    public int CategoryId { get; init; }
    public DateTime TransactionDate {  get; init; }
    public TransactionType TransactionType { get; init; }
    public string? Note { get; init; } 
}

public enum TransactionType
{
    Outcome = 0,
    Income = 1,
    Transfer = 2
}
