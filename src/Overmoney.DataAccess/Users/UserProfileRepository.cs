using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Users;

internal sealed class UserProfileRepository : IUserProfileRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserProfileRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<UserProfile> CreateAsync(UserProfile user, CancellationToken token)
    {
        var entity = _databaseContext.Add(new UserProfileEntity(user.Email));
        await _databaseContext.SaveChangesAsync(token);

        return new UserProfile(entity.Entity.Id, entity.Entity.Email);
    }

    public async Task DeleteByIdAsync(UserProfileId userId, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Users
            .Where(x => x.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<UserProfile?> GetByEmailAsync(string email, CancellationToken token)
    {
        var user = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == email, token);

        if (user is null)
        {
            return null;
        }

        return new UserProfile(user.Id,user.Email);
    }

    public async Task<UserProfile?> GetByIdAsync(UserProfileId userId, CancellationToken token)
    {
        var user = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId, token);

        if (user is null)
        {
            return null;
        }

        return new UserProfile(user.Id, user.Email);
    }
}
