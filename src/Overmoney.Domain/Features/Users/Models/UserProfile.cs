using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Users.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class UserProfileId : Identity<int>
{
    public UserProfileId(int value) : base(value)
    {
    }
}

public sealed class UserProfile
{
    public UserProfileId? Id { get; }
    public string Email { get; } = null!;

    public UserProfile(UserProfileId id, string email)
        : this(email)
    {
        Id = id;
    }

    public UserProfile(string email)
    {
        Email = email;
    }
}
