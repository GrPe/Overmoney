using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.DataAccess.Categories;
internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseContext _databaseContext;

    public CategoryRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.SingleAsync(x => x.Id == category.UserId, cancellationToken);
        var entity = _databaseContext.Add(new CategoryEntity(user, category.Name));

        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new Category(entity.Entity.Id, entity.Entity.UserId, entity.Entity.Name);
    }

    public async Task DeleteAsync(CategoryId id, CancellationToken cancellationToken)
    {
        await _databaseContext
            .Categories
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await _databaseContext
            .Categories
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new Category(x.Id, x.UserId, x.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetAsync(CategoryId id, CancellationToken cancellationToken)
    {
        var category = await _databaseContext
            .Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return null;
        }

        return new Category(category.Id, category.UserId, category.Name);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        var entity = await _databaseContext
            .Categories
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == category.Id!, cancellationToken);

        if (entity is null)
        {
            return;
        }

        var user = entity.UserId == category.UserId
            ? entity.User
            : await _databaseContext.Users.SingleAsync(x => x.Id == category.UserId, cancellationToken);

        entity.Update(user, category.Name);
        _databaseContext.Update(entity);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
