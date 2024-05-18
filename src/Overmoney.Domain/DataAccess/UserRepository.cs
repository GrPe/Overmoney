using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface IUserProfileRepository : IRepository
{
    Task<UserProfile> CreateAsync(UserProfile user, CancellationToken token);
    Task DeleteByIdAsync(UserProfileId userId, CancellationToken cancellationToken);
    Task<UserProfile?> GetByEmailAsync(string email, CancellationToken token);
    Task<UserProfile?> GetByIdAsync(UserProfileId userId, CancellationToken token);
}