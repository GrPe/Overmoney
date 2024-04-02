using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface IUserRepository : IRepository
{
    Task<UserId> CreateAsync(User user, CancellationToken token);
    Task DeleteByIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken token);
    Task<User?> GetByLoginAsync(string? login, CancellationToken cancellationToken);
}