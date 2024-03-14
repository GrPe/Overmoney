using Overmoney.Api.Features;
using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.DataAccess.Categories;

public interface ICategoryRepository : IRepository
{
    Task<CategoryEntity?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CategoryEntity>> GetAllByUserAsync(int userId, CancellationToken cancellationToken);
    Task<CategoryEntity> CreateAsync(CreateCategory category, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateCategory category, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

public sealed class CategoryRepository : ICategoryRepository
{
    private static readonly List<CategoryEntity> _connection = [new(1, 1, "Test Category")];

    public async Task<CategoryEntity> CreateAsync(CreateCategory category, CancellationToken cancellationToken)
    {
        _connection.Add(new(_connection.Max(x => x.Id) + 1, category.UserId, category.Name));
        return await Task.FromResult(_connection.Last());
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = _connection.FirstOrDefault(x => x.Id == id);

        if (category is null)
        {
            return Task.CompletedTask;
        }

        _connection.Remove(category);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId));
    }

    public async Task<CategoryEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.FirstOrDefault(x => x.Id == id));
    }

    public Task UpdateAsync(UpdateCategory category, CancellationToken cancellationToken)
    {
        var old = _connection.FirstOrDefault(x => x.Id == category.Id);
        _connection.Add(new(category.Id, old!.UserId, category.Name));
        _connection.Remove(old);
        return Task.CompletedTask;
    }
}
