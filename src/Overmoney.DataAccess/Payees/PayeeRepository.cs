﻿using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Payees;
internal sealed class PayeeRepository : IPayeeRepository
{
    private readonly DatabaseContext _databaseContext;

    public PayeeRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Payee> CreateAsync(Payee payee, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == payee.UserId, cancellationToken);
        var entity = _databaseContext.Add(new PayeeEntity(user, payee.Name));

        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new Payee(entity.Entity.Id, entity.Entity.UserId, entity.Entity.Name);
    }

    public async Task DeleteAsync(PayeeId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Payees
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payee>> GetAllByUserIdAsync(UserProfileId userId, CancellationToken cancellationToken)
    {
        return await _databaseContext
            .Payees
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new Payee(x.Id, x.UserId, x.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Payee?> GetAsync(PayeeId id, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Payees
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new Payee(entity.Id, entity.UserId, entity.Name);
    }

    public async Task UpdateAsync(Payee payee, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Payees
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == payee.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        var user = entity.UserId == payee.UserId
            ? entity.User
            : await _databaseContext.Users.SingleAsync(x => x.Id == payee.UserId, cancellationToken);

        entity.Update(user, payee.Name);
        _databaseContext.Update(user);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
