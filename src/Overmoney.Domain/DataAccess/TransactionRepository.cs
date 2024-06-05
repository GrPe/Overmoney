using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.Domain.DataAccess;

public interface ITransactionRepository : IRepository
{
    Task<bool> IsExists(TransactionId id, CancellationToken cancellationToken);
    Task<Transaction?> GetAsync(TransactionId id, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetUserTransactionsAsync(UserProfileId userId, WalletId? walletId, CategoryId? categoryId, PayeeId? payeeId, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(TransactionId id, CancellationToken cancellationToken);

    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(UserProfileId userId, CancellationToken cancellationToken);
    Task<RecurringTransaction?> GetRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken);
    Task DeleteRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken);
    Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(AttachmentId id, CancellationToken cancellationToken);
    Task<Attachment?> GetAttachmentAsync(AttachmentId id, CancellationToken cancellationToken);
    Task<Attachment> AddAttachmentAsync(TransactionId transactionId, Attachment attachment, CancellationToken cancellationToken);
    Task UpdateAttachmentAsync(Attachment attachment, CancellationToken cancellationToken);
}