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

    public static string GenerateCategory()
    {
        return new Faker().Commerce.Categories(1).First();
    }

    public static string GenerateWallet()
    {
        return new Faker().Finance.AccountName();
    }

    public static (double Amount, string Note, DateTime TransactionDate) GenerateTransaction()
    {
        var amount = new Faker().Finance.Amount();
        var note = new Faker().Lorem.Sentence();
        var date = new Faker().Date.Past().ToUniversalTime();
        return (((double)amount), note, date);
    }
}
