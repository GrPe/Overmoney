using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.Domain.DataAccess;

public interface ICategoryRepository : IRepository
{
    Task<Category?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllByUserAsync(int userId, CancellationToken cancellationToken);
    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}