namespace Overmoney.Api.Features.Categories.Models;

public sealed class UpdateCategory
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;

    public UpdateCategory(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
