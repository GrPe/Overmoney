namespace Overmoney.Api.DataAccess.Categories.Models;

public sealed class CreateCategory
{
    public int UserId { get; init; }
    public string Name { get; init; } = null!;

    public CreateCategory(int userId, string name)
    {
        UserId = userId;
        Name = name;
    }
}
