using Overmoney.Api.Features;
using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.DataAccess.Categories;

public interface ICategoryRepository : IRepository
{
    Task<Category?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllByUserAsync(int userId, CancellationToken cancellationToken);
    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

internal sealed class CategoryRepository : ICategoryRepository
{
    private static readonly List<CategoryEntity> _connection = [new(1, 1, "Test Category")];

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken)
    {
        var entity = new CategoryEntity(_connection.Max(x => x.Id) + 1, category.UserId, category.Name);
        _connection.Add(entity);
        return new Category(entity.Id, entity.UserId, entity.Name);
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

    public async Task<IEnumerable<Category>> GetAllByUserAsync(int userId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_connection.Where(x => x.UserId == userId).Select(x => new Category(x.Id, x.UserId, x.Name)));
    }

    public async Task<Category?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var category = _connection.FirstOrDefault(x => x.Id == id);
        if(category is null)
        {
            return null;
        }
        return new Category(category.Id, category.UserId, category.Name);
    }

    public Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        var old = _connection.FirstOrDefault(x => x.Id == category.Id);
        if(old is not null)
        {
            _connection.Remove(old);
        }
        _connection.Add(new(old!.Id, category.UserId, category.Name));
        return Task.CompletedTask;
    }
}
