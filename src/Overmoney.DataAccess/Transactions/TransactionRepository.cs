using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Currencies.Models;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;

namespace Overmoney.DataAccess.Transactions;

internal sealed class TransactionRepository : ITransactionRepository
{
    private readonly DatabaseContext _databaseContext;

    public TransactionRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == transaction.UserId, cancellationToken);
        var category = await _databaseContext.Categories.SingleAsync(x => x.Id == transaction.Category.Id, cancellationToken);
        var payee = await _databaseContext.Payees.SingleAsync(x => x.Id == transaction.Payee.Id, cancellationToken);
        var wallet = await _databaseContext.Wallets.SingleAsync(x => x.Id == transaction.Wallet.Id, cancellationToken);

        var entity = _databaseContext.Add(new TransactionEntity(wallet, user, payee, category, new DateTime(transaction.TransactionDate, new(), DateTimeKind.Utc), transaction.TransactionType, transaction.Note, transaction.Amount));

        foreach (var attachment in transaction.Attachments)
        {
            entity.Entity.Attachments.Add(new AttachmentEntity(entity.Entity, attachment.Name, attachment.FilePath));
        }

        await _databaseContext.SaveChangesAsync(cancellationToken);

        return (await GetAsync(entity.Entity.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(TransactionId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Transactions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteAttachmentAsync(AttachmentId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Attachments
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Transaction?> GetAsync(TransactionId id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Transactions
            .AsNoTracking()
            .Include(x => x.Wallet)
            .ThenInclude(w => w.Currency)
            .Include(x => x.Category)
            .Include(x => x.Payee)
            .Include(x => x.Attachments)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Transaction(
            entity.Id,
            entity.UserId,
            new Wallet(entity.Wallet.Id, entity.Wallet.Name, new Currency(entity.Wallet.CurrencyId, entity.Wallet.Currency.Code, entity.Wallet.Currency.Name), entity.Wallet.UserId),
            new Payee(entity.Payee.Id, entity.Payee.UserId, entity.Payee.Name),
            new Category(entity.Category.Id, entity.Category.UserId, entity.Category.Name),
            DateOnly.FromDateTime(entity.TransactionDate),
            entity.TransactionType,
            entity.Note,
            entity.Amount,
            entity.Attachments.Select(x => new Attachment(x.Id, x.Name, x.FilePath)).ToList());
    }

    public async Task<Attachment?> GetAttachmentAsync(AttachmentId id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Attachments
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Attachment(entity.Id, entity.Name, entity.FilePath);
    }

    public async Task<bool> IsExists(TransactionId id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Transactions
            .AsNoTracking()
            .Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id, cancellationToken);

        return entity is not null && entity.Value != 0;
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
           .Transactions
           .Include(x => x.Wallet)
           .Include(x => x.Category)
           .Include(x => x.Payee)
           .Include(x => x.Attachments)
           .SingleOrDefaultAsync(x => x.Id == transaction.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        var category = entity.CategoryId == transaction.Category.Id
            ? entity.Category
            : await _databaseContext.Categories.SingleAsync(x => x.Id == transaction.Category.Id, cancellationToken);

        var payee = entity.PayeeId == transaction.Payee.Id
            ? entity.Payee
            : await _databaseContext.Payees.SingleAsync(x => x.Id == transaction.Payee.Id, cancellationToken);

        var wallet = entity.WalletId == transaction.Wallet.Id
            ? entity.Wallet
            : await _databaseContext.Wallets.SingleAsync(x => x.Id == transaction.Wallet.Id, cancellationToken);

        entity.Update(wallet, payee, category, new DateTime(transaction.TransactionDate, new(), DateTimeKind.Utc), transaction.TransactionType, transaction.Note, transaction.Amount);
        _databaseContext.Update(entity);


        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAttachmentAsync(TransactionId transactionId, Attachment attachment, CancellationToken cancellationToken)
    {
        var transaction = await _databaseContext
            .Transactions
            .SingleAsync(x => x.Id == transactionId, cancellationToken);

        var entity = new AttachmentEntity(transaction, attachment.Name, attachment.FilePath);

        _databaseContext.Add(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == transaction.UserId, cancellationToken);
        var wallet = await _databaseContext.Wallets.SingleAsync(x => x.Id == transaction.Wallet.Id, cancellationToken);
        var payee = await _databaseContext.Payees.SingleAsync(x => x.Id == transaction.Payee.Id, cancellationToken);
        var category = await _databaseContext.Categories.SingleAsync(x => x.Id == transaction.Category.Id, cancellationToken);

        var entity = _databaseContext.Add(new RecurringTransactionEntity(wallet, user, payee, category, transaction.NextOccurrence, transaction.TransactionType, transaction.Note, transaction.Amount, transaction.Schedule));

        await _databaseContext.SaveChangesAsync(cancellationToken);
        return (await GetRecurringTransactionAsync(entity.Entity.Id, cancellationToken))!;
    }

    public async Task<RecurringTransaction?> GetRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .RecurringTransactions
            .AsNoTracking()
            .Include(x => x.Wallet)
            .ThenInclude(w => w.Currency)
            .Include(x => x.Category)
            .Include(x => x.Payee)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new RecurringTransaction(
            entity.Id,
            entity.UserId,
            new Wallet(entity.Wallet.Id, entity.Wallet.Name, new Currency(entity.Wallet.CurrencyId, entity.Wallet.Currency.Code, entity.Wallet.Currency.Name), entity.Wallet.UserId),
            new Payee(entity.Payee.Id, entity.Payee.UserId, entity.Payee.Name),
            new Category(entity.Category.Id, entity.Category.UserId, entity.Category.Name),
            entity.TransactionType,
            entity.Note,
            entity.Amount,
            entity.Schedule,
            entity.NextOccurrence);
    }

    public async Task DeleteRecurringTransactionAsync(RecurringTransactionId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .RecurringTransactions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
           .RecurringTransactions
           .Include(x => x.Wallet)
           .Include(x => x.Category)
           .Include(x => x.Payee)
           .Include(x => x.User)
           .SingleOrDefaultAsync(x => x.Id == transaction.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        var category = entity.CategoryId == transaction.Category.Id
            ? entity.Category
            : await _databaseContext.Categories.SingleAsync(x => x.Id == transaction.Category.Id, cancellationToken);

        var payee = entity.PayeeId == transaction.Payee.Id
            ? entity.Payee
            : await _databaseContext.Payees.SingleAsync(x => x.Id == transaction.Payee.Id, cancellationToken);

        var wallet = entity.WalletId == transaction.Wallet.Id
            ? entity.Wallet
            : await _databaseContext.Wallets.SingleAsync(x => x.Id == transaction.Wallet.Id, cancellationToken);

        entity.Update(wallet, payee, category, transaction.NextOccurrence, transaction.TransactionType, transaction.Note, transaction.Amount, transaction.Schedule);
        _databaseContext.Update(entity);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(UserProfileId userId, CancellationToken cancellationToken)
    {
        var entities = await _databaseContext
            .RecurringTransactions
            .AsNoTracking()
            .Include(x => x.Wallet)
            .ThenInclude(w => w.Currency)
            .Include(x => x.Category)
            .Include(x => x.Payee)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        return entities.Select(x => new RecurringTransaction(
            x.Id,
            x.UserId,
            new Wallet(x.Wallet.Id, x.Wallet.Name, new Currency(x.Wallet.CurrencyId, x.Wallet.Currency.Code, x.Wallet.Currency.Name), x.Wallet.UserId),
            new Payee(x.Payee.Id, x.Payee.UserId, x.Payee.Name),
            new Category(x.Category.Id, x.Category.UserId, x.Category.Name),
            x.TransactionType,
            x.Note,
            x.Amount,
            x.Schedule,
            x.NextOccurrence));
    }

    public async Task UpdateAttachmentAsync(Attachment attachment, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Attachments
            .SingleOrDefaultAsync(x => x.Id == attachment.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        entity.Update(attachment.Name);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(UserProfileId userId, CancellationToken cancellationToken)
    {
        return await _databaseContext
            .Transactions
            .AsNoTracking()
            .Include(x => x.Wallet)
            .ThenInclude(w => w.Currency)
            .Include(x => x.Category)
            .Include(x => x.Payee)
            .Include(x => x.Attachments)
            .Where(x => x.UserId == userId)
            .Select(x => new Transaction(
                x.Id,
                x.UserId,
                new Wallet(
                    x.Wallet.Id, 
                    x.Wallet.Name, 
                    new Currency(x.Wallet.CurrencyId, x.Wallet.Currency.Code, x.Wallet.Currency.Name), 
                    x.Wallet.UserId),
                new Payee(x.Payee.Id, x.Payee.UserId, x.Payee.Name),
                new Category(x.Category.Id, x.Category.UserId, x.Category.Name),
                DateOnly.FromDateTime(x.TransactionDate),
                x.TransactionType,
                x.Note,
                x.Amount,
                x.Attachments.Select(a => new Attachment(a.Id, a.Name, a.FilePath)).ToList()))
            .ToListAsync(cancellationToken);
    }
}
