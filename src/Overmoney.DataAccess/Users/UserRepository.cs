using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Users;

internal sealed class UserRepository : IUserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<int> CreateAsync(User user, CancellationToken token)
    {
        var entity = _databaseContext.Add(new UserEntity(user.Login, user.Email, user.Password));
        await _databaseContext.SaveChangesAsync(token);

        return entity.Entity.Id;
    }

    public async Task DeleteByIdAsync(int userId, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Users
            .Where(x => x.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
    {
        var user = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == email, token);

        if (user is null)
        {
            return null;
        }

        return new User(user.Id, user.Login, user.Email, user.Password);
    }

    public async Task<User?> GetByIdAsync(int userId, CancellationToken token)
    {
        var user = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId, token);

        if (user is null)
        {
            return null;
        }

        return new User(user.Id, user.Login, user.Email, user.Password);
    }

    public async Task<User?> GetByLoginAsync(string? login, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return new User(user.Id, user.Login, user.Email, user.Password);
    }
}
