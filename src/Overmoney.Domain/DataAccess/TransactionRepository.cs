using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.DataAccess;

public interface ITransactionRepository : IRepository
{
    Task<bool> IsExists(long id, CancellationToken cancellationToken);
    Task<Transaction?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<RecurringTransaction?> GetRecurringTransactionAsync(long id, CancellationToken cancellationToken);
    Task DeleteRecurringTransactionAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(long id, CancellationToken cancellationToken);
    Task<Attachment?> GetAttachmentAsync(long id, CancellationToken cancellationToken);
    Task AddAttachmentAsync(long transactionId, Attachment attachment, CancellationToken cancellationToken);
    Task UpdateAttachmentAsync(Attachment attachment, CancellationToken cancellationToken);
}