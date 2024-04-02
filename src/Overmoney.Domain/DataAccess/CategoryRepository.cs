using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.DataAccess;

public interface ICategoryRepository : IRepository
{
    Task<Category?> GetAsync(CategoryId id, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken);
    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(CategoryId id, CancellationToken cancellationToken);
}