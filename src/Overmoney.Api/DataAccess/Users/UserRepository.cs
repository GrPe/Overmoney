using Overmoney.Api.DataAccess.Users.Models;

namespace Overmoney.Api.DataAccess.Users;

public interface IUserRepository : IRepository
{
    Task CreateAsync(CreateUser user, CancellationToken token);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
}

internal sealed class UserRepository : IUserRepository
{
    private static readonly List<User> _connection = [new(1, "test", "test@test.test", "test")];

    public async Task CreateAsync(CreateUser user, CancellationToken token)
    {
        _connection.Add(new User(_connection.Max(x => x.Id) + 1, user.UserName, user.Email, user.Password));
        await Task.CompletedTask;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Email == email));
    }
}
