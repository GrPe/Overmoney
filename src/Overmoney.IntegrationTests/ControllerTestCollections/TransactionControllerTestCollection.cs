using Overmoney.IntegrationTests.Configurations;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class TransactionControllerTestCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;

    public TransactionControllerTestCollection(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
        _fixture = fixture;
    }

    [Fact]
    public async Task When_correct_data_are_provided_transaction_should_be_created()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
        content.Amount.ShouldBe(transaction.Amount);
        content.TransactionDate.ShouldBe(new DateTime(transaction.TransactionDate, new(), DateTimeKind.Utc));
        content.Note.ShouldBe(transaction.Note);
        content.UserId.ShouldBe(userId);
        content.Wallet?.Id.ShouldBe(walletId);
        content.Category?.Id.ShouldBe(categoryId);
        content.Payee?.Id.ShouldBe(payeeId);
    }

    [Fact]
    public async Task When_correct_data_are_provided_transactions_should_be_created()
    {
        var userId = await _fixture.GetRandomUser();
        foreach (var _ in Enumerable.Range(0, 10))
        {
            var walletId = await _fixture.GetRandomWallet(userId);
            var categoryId = await _fixture.GetRandomCategory(userId);
            var payeeId = await _fixture.GetRandomPayee(userId);

            var transaction = DataFaker.GenerateTransaction();
            var response = await _client
                .PostAsJsonAsync("transactions", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

            response.IsSuccessStatusCode.ShouldBeTrue();

            var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

            content.ShouldNotBeNull();
            content.Id.ShouldBeGreaterThan(0);
            content.Amount.ShouldBe(transaction.Amount);
            content.TransactionDate.ShouldBe(new DateTime(transaction.TransactionDate, new(), DateTimeKind.Utc));
            content.Note.ShouldBe(transaction.Note);
            content.UserId.ShouldBe(userId);
            content.Wallet?.Id.ShouldBe(walletId);
            content.Category?.Id.ShouldBe(categoryId);
            content.Payee?.Id.ShouldBe(payeeId);
        }
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_api_should_return_bad_request()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = userId, WalletId = walletId, CategoryId = int.MaxValue, PayeeId = payeeId, Amount = 0, TransactionDate = DateTime.UtcNow, Note = "", TransactionType = 0 });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_transaction_exists_and_update_request_is_sent_then_transaction_should_be_updated()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

        var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        var updatedTransaction = DataFaker.GenerateTransaction();
        var putResponse = await _client
            .PutAsJsonAsync($"transactions", new { content!.Id, UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, updatedTransaction.Amount, updatedTransaction.TransactionDate, updatedTransaction.Note, TransactionType = 0 });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();

        var transactionResponse = await _client
            .GetFromJsonAsync<TransactionResponse>($"transactions/{content.Id}");

        transactionResponse.ShouldNotBeNull();
        transactionResponse.Id.ShouldBe(content.Id);
        transactionResponse.Amount.ShouldBe(updatedTransaction.Amount);
        transactionResponse.TransactionDate.ShouldBe(new DateTime(updatedTransaction.TransactionDate, new(), DateTimeKind.Utc));
        transactionResponse.Note.ShouldBe(updatedTransaction.Note);
        transactionResponse.UserId.ShouldBe(userId);
        transactionResponse.Wallet?.Id.ShouldBe(walletId);
        transactionResponse.Category?.Id.ShouldBe(categoryId);
        transactionResponse.Payee?.Id.ShouldBe(payeeId);
    }

    [Fact]
    public async Task When_transaction_exists_and_delete_request_is_sent_then_transaction_should_be_deleted()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

        var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        var deleteResponse = await _client
            .DeleteAsync($"transactions/{content!.Id}");

        deleteResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

}

record TransactionResponse(int Id, int UserId, WalletResponse Wallet, CategoryResponse Category, PayeeResponse Payee, decimal Amount, DateTime TransactionDate, string Note);
