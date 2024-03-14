namespace Overmoney.Api.Features.Users.Models;

public sealed class User
{
    public int? Id { get; }
    public string Login { get; } = null!;
    public string Email { get; } = null!;
    public string Password { get; } = null!;

    public User(int id, string login, string email, string password)
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
