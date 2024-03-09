namespace Overmoney.Api.DataAccess.Categories;

public class Category
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public Category(int id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
