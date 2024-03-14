namespace Overmoney.Api.Features.Categories.Models;

public sealed class Category
{
    public int? Id { get; init; }
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public Category(int id, int userId, string name) : this(userId, name)
    {
        Id = id;
    }

    public Category(int userId, string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        UserId = userId;
        Name = name;
    }
}
