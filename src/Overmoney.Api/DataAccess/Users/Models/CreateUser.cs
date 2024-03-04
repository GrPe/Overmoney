namespace Overmoney.Api.DataAccess.Users.Models;

public sealed class CreateUser
{
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public CreateUser(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }
}
