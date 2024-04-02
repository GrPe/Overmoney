using Overmoney.Domain.Converters;
using Overmoney.Domain.Features.Common.Models;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Features.Users.Models;

[JsonConverter(typeof(IntIdentityJsonConverter))]
public sealed class UserId : Identity<int>
{
    public UserId(int value) : base(value)
    {
    }
}

public sealed class User
{
    public UserId? Id { get; }
    public string Login { get; } = null!;
    public string Email { get; } = null!;
    public string Password { get; } = null!;

    public User(UserId id, string login, string email, string password)
        : this(login, email, password)
    {
        Id = id;
    }

    public User(string login, string email, string password)
    {
        Login = login;
        Email = email;
        Password = password;
    }
}
