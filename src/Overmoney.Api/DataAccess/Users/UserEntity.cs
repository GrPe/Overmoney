﻿namespace Overmoney.Api.DataAccess.Users;

internal sealed class UserEntity
{
    public int Id { get; init; }
    public string Login { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public UserEntity(int id, string login, string email, string password)
    {
        Id = id;
        Login = login;
        Email = email;
        Password = password;
    }
}
