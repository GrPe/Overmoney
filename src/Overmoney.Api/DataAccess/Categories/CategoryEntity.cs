namespace Overmoney.Api.DataAccess.Categories;

public sealed class CategoryEntity
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public CategoryEntity(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
