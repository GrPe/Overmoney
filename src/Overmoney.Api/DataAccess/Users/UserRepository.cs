using Overmoney.Api.Features;
using Overmoney.Api.Features.Users.Models;

namespace Overmoney.Api.DataAccess.Users;

public interface IUserRepository : IRepository
{
    Task<int> CreateAsync(User user, CancellationToken token);
    Task DeleteByIdAsync(int userId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByIdAsync(int userId, CancellationToken token);
    Task<User?> GetByLoginAsync(string? login, CancellationToken cancellationToken);
}

internal sealed class UserRepository : IUserRepository
{
    private static readonly List<UserEntity> _connection = [new(1, "test", "test@test.test", "test")];

    public async Task<int> CreateAsync(User user, CancellationToken token)
    {
        _connection.Add(new UserEntity(_connection.Max(x => x.Id) + 1, user.Login, user.Email, user.Password));
        return await Task.FromResult(_connection.Max(x => x.Id));
    }

    public async Task DeleteByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var user = _connection.FirstOrDefault(x => x.Id == userId);

        if (user is null)
        {
            return;
        }

        _connection.Remove(user);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
    {
        var user = _connection.FirstOrDefault(x => x.Email == email);
        if (user is null)
        {
            return null;
        }
        return await Task.FromResult(new User(user.Id, user.Login, user.Email, user.Password));
    }

    public async Task<User?> GetByIdAsync(int userId, CancellationToken token)
    {
        var user = _connection.FirstOrDefault(x => x.Id == userId);
        if (user is null)
        {
            return null;
        }
        return await Task.FromResult(new User(user.Id, user.Login, user.Email, user.Password));
    }

    public async Task<User?> GetByLoginAsync(string? login, CancellationToken cancellationToken)
    {
        var user = _connection.FirstOrDefault(x => x.Login == login);
        if(user is null)
        {
            return null;
        }
        return await Task.FromResult(new User(user.Id, user.Login, user.Email, user.Password));
    }
}
