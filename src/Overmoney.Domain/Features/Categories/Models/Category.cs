using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using Overmoney.Domain.Features.Users.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Categories.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class CategoryId : Identity<int>
{
    public CategoryId(int value) : base(value)
    {
    }
}
public sealed class Category
{
    public CategoryId? Id { get; init; }
    public UserProfileId UserId { get; init; } = null!;
    public string Name { get; init; } = null!;

    public Category(CategoryId id, UserProfileId userId, string name) : this(userId, name)
    {
        Id = id;
    }

    public Category(UserProfileId userId, string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        UserId = userId;
        Name = name;
    }
}
