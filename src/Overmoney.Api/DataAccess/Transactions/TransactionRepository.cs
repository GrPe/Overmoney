using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions;

public interface ITransactionRepository : IRepository
{
    Task<Transaction?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(CreateTransaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateTransaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class TransactionRepository : ITransactionRepository
{
    private static readonly List<Transaction> _connection = [new(1, 1, 1, 1, 1, DateTime.UtcNow, TransactionType.Outcome, "Onion", .3)];

    public async Task<Transaction> CreateAsync(CreateTransaction transaction, CancellationToken cancellationToken)
    {
        _connection.Add(new Transaction(_connection.Max(x => x.Id) + 1, transaction.WalletId, transaction.UserId, transaction.PayeeId, transaction.CategoryId, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount));
        return await Task.FromResult(_connection.Last());
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var transaction = _connection.FirstOrDefault(x => x.Id == id);
        if (transaction != null)
        {
            _connection.Remove(transaction);
        }
    }

    public async Task<Transaction?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public Task UpdateAsync(UpdateTransaction transaction, CancellationToken cancellationToken)
    {
        var oldTransaction = _connection.First(x => x.Id == transaction.Id);
        var newTransaction = new Transaction(oldTransaction.Id, transaction.WalletId, transaction.UserId, transaction.PayeeId, transaction.CategoryId, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount);
        _connection.Remove(oldTransaction);
        _connection.Add(newTransaction);

        return Task.CompletedTask;
    }
}
