using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface ITransactionRepository : IRepository
{
    Task<bool> IsExists(TransactionId id, CancellationToken cancellationToken);
    Task<Transaction?> GetAsync(TransactionId id, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(TransactionId id, CancellationToken cancellationToken);

    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<RecurringTransaction?> GetRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken);
    Task DeleteRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken);
    Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(AttachmentId id, CancellationToken cancellationToken);
    Task<Attachment?> GetAttachmentAsync(AttachmentId id, CancellationToken cancellationToken);
    Task AddAttachmentAsync(TransactionId transactionId, Attachment attachment, CancellationToken cancellationToken);
    Task UpdateAttachmentAsync(Attachment attachment, CancellationToken cancellationToken);
}