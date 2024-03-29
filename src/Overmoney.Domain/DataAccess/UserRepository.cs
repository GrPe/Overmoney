using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface IUserRepository : IRepository
{
    Task<int> CreateAsync(User user, CancellationToken token);
    Task DeleteByIdAsync(int userId, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken token);
    Task<User?> GetByIdAsync(int userId, CancellationToken token);
    Task<User?> GetByLoginAsync(string? login, CancellationToken cancellationToken);
}