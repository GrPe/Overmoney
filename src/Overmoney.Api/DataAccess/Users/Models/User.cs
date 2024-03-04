namespace Overmoney.Api.DataAccess.Users.Models;

public sealed class User
{
    public int Id { get; init; }
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public User(int id, string userName, string email, string password)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Password = password;
    }
}
