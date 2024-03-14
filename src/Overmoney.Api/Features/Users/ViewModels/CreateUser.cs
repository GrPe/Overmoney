namespace Overmoney.Api.Features.Users.Models;

public sealed class CreateUser
{
    public string Login { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public CreateUser(string login, string email, string password)
    {
        Login = login;
        Email = email;
        Password = password;
    }
}
