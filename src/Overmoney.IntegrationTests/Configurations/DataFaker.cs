using Bogus;

namespace Overmoney.IntegrationTests.Configurations;
internal static class DataFaker
{
    public static (string UserName, string Email, string Password) GenerateUser()
    {
        var userName = new Faker().Name.FirstName();
        var email = new Faker().Internet.Email();
        var password = new Faker().Internet.Password();
        return (userName, email, password);
    }

    public static (string Name, string Code) GenerateCurrency()
    {
        var currency = new Faker().Finance.Currency();
        return (currency.Description, currency.Code);
    }

    public static string GeneratePayee()
    {
        return new Faker().Company.CompanyName();
    }
}
