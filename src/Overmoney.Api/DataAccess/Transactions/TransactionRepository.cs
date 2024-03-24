using Microsoft.EntityFrameworkCore;
using Overmoney.Api.Features;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.DataAccess.Transactions;

public interface ITransactionRepository : IRepository
{
    Task<bool> IsExists(long id, CancellationToken cancellationToken);
    Task<Transaction?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<RecurringTransaction> GetRecurringTransactionAsync(long id, CancellationToken cancellationToken);
    Task DeleteRecurringTransactionAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(long id, CancellationToken cancellationToken);
    Task<Attachment?> GetAttachmentAsync(long id, CancellationToken cancellationToken);
    Task AddAttachmentAsync(long transactionId, Attachment attachment, CancellationToken cancellationToken);
}

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

        var entity = _databaseContext.Add(new TransactionEntity(wallet, user, payee, category, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount));

        foreach (var attachment in transaction.Attachments)
        {
            entity.Entity.Attachments.Add(new AttachmentEntity(entity.Entity, attachment.Name, attachment.FilePath));
        }

        await _databaseContext.SaveChangesAsync(cancellationToken);

        return (await GetAsync(entity.Entity.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Transactions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteAttachmentAsync(long id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Attachments
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Transaction?> GetAsync(long id, CancellationToken cancellationToken)
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
            new Features.Wallets.Models.Wallet(entity.Wallet.Id, entity.Wallet.Name, new Features.Currencies.Models.Currency(entity.Wallet.CurrencyId, entity.Wallet.Currency.Code, entity.Wallet.Currency.Name), entity.Wallet.UserId),
            new Features.Payees.Models.Payee(entity.Payee.Id, entity.Payee.UserId, entity.Payee.Name),
            new Features.Categories.Models.Category(entity.Category.Id, entity.Category.UserId, entity.Category.Name),
            entity.TransactionDate,
            entity.TransactionType,
            entity.Note,
            entity.Amount,
            entity.Attachments.Select(x => new Attachment(x.Id, x.Name, x.FilePath)).ToList());
    }

    public async Task<Attachment?> GetAttachmentAsync(long id, CancellationToken cancellationToken)
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

    public async Task<bool> IsExists(long id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Transactions
            .AsNoTracking()
            .Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id, cancellationToken);

        return entity != 0;
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
           .Transactions
           .Include(x => x.Wallet)
           .Include(x => x.Category)
           .Include(x => x.Payee)
           .Include(x => x.User)
           .Include(x => x.Attachments)
           .SingleOrDefaultAsync(x => x.Id == transaction.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        var user = entity.UserId == transaction.UserId
            ? entity.User
            : await _databaseContext.Users.SingleAsync(x => x.Id == transaction.UserId, cancellationToken);

        var category = entity.CategoryId == transaction.Category.Id
            ? entity.Category
            : await _databaseContext.Categories.SingleAsync(x => x.Id == transaction.Category.Id, cancellationToken);

        var payee = entity.PayeeId == transaction.Payee.Id
            ? entity.Payee
            : await _databaseContext.Payees.SingleAsync(x => x.Id == transaction.Payee.Id, cancellationToken);

        var wallet = entity.WalletId == transaction.Wallet.Id
            ? entity.Wallet
            : await _databaseContext.Wallets.SingleAsync(x => x.Id == transaction.Wallet.Id, cancellationToken);

        entity.Update(wallet, user, payee, category, transaction.TransactionDate, transaction.TransactionType, transaction.Note, transaction.Amount);
        _databaseContext.Update(entity);


        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAttachmentAsync(long transactionId, Attachment attachment, CancellationToken cancellationToken)
    {
        var transaction = await _databaseContext
            .Transactions
            .SingleAsync(x => x.Id == transactionId, cancellationToken);

        var entity = new AttachmentEntity(transaction, attachment.Name, attachment.FilePath);

        _databaseContext.Add(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<RecurringTransaction> GetRecurringTransactionAsync(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRecurringTransactionAsync(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(RecurringTransaction transaction, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RecurringTransaction>> GetRecurringTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
