using Overmoney.Api.Features;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.DataAccess.Transactions;

public interface ITransactionRepository : IRepository
{
    Task<Transaction?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class TransactionRepository : ITransactionRepository
{
    private static readonly List<TransactionEntity> _connection = [new(1, 1, 1, 1, 1, DateTime.UtcNow, TransactionType.Outcome, "Onion", .3)];

    public async Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var entity = new TransactionEntity(_connection.Max(x => x.Id) + 1, transaction.WalletId, transaction.UserId, transaction.PayeeId, transaction.CategoryId, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount);
        _connection.Add(entity);
        return (await GetAsync(entity.Id, cancellationToken))!;
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var transaction = _connection.FirstOrDefault(x => x.Id == id);
        if (transaction != null)
        {
            _connection.Remove(transaction);
        }
        return Task.CompletedTask;
    }

    public async Task<Transaction?> GetAsync(long id, CancellationToken cancellationToken)
    {
        var entity = _connection.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return null;
        }
        return await Task.FromResult(new Transaction(entity.Id, entity.WalletId, entity.UserId, entity.PayeeId, entity.CategoryId, entity.TransactionDate, entity.TransactionType, entity.Note, entity.Amount));
    }

    public Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var oldTransaction = _connection.First(x => x.Id == transaction.Id);
        var newTransaction = new TransactionEntity(oldTransaction.Id, transaction.WalletId, transaction.UserId, transaction.PayeeId, transaction.CategoryId, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount);
        _connection.Remove(oldTransaction);
        _connection.Add(newTransaction);

        return Task.CompletedTask;
    }
}
