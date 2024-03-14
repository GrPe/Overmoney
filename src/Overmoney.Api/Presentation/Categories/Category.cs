namespace Overmoney.Api.Presentation.Categories;

public sealed class Category
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
