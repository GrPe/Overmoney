using FluentAssertions;
using Overmoney.IntegrationTests.Configurations;
using Shouldly;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class RecurringTransactionControllerTestCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;

    public RecurringTransactionControllerTestCollection(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
        _fixture = fixture;
    }

    [Fact]
    public async Task When_correct_data_are_provided_recurring_transaction_should_be_created()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateRecurringTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions/recurring", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.FirstOccurrence, transaction.Note, TransactionType = 0, transaction.Schedule });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<RecurringTransactionResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
        content.Amount.ShouldBe(transaction.Amount);
        content.NextOccurrence.ShouldBe(transaction.FirstOccurrence, TimeSpan.FromSeconds(1));
        content.Note.ShouldBe(transaction.Note);
        content.UserId.ShouldBe(userId);
        content.Wallet?.Id.ShouldBe(walletId);
        content.Category?.Id.ShouldBe(categoryId);
        content.Payee?.Id.ShouldBe(payeeId);
        content.Schedule.ShouldBe(transaction.Schedule);
    }

    [Fact]
    public async Task When_request_without_payeeId_is_sent_when_recurring_transaction_cannot_be_created()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);

        var transaction = DataFaker.GenerateRecurringTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions/recurring", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, transaction.Amount, transaction.FirstOccurrence, transaction.Note, TransactionType = 0, transaction.Schedule });

        response.IsSuccessStatusCode.ShouldBeFalse();
    }

    [Fact]
    public async Task When_transaction_exists_and_update_request_is_sent_then_transaction_should_be_updated()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateRecurringTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions/recurring", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.FirstOccurrence, transaction.Note, TransactionType = 0, transaction.Schedule });

        var content = await response.Content.ReadFromJsonAsync<RecurringTransactionResponse>();

        var updatedTransaction = DataFaker.GenerateRecurringTransaction();
        var putResponse = await _client
            .PutAsJsonAsync($"transactions/recurring", new { content!.Id, UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, updatedTransaction.Amount, updatedTransaction.FirstOccurrence, updatedTransaction.Note, TransactionType = 0, updatedTransaction.Schedule });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();

        var transactionResponse = await _client
            .GetFromJsonAsync<RecurringTransactionResponse>($"transactions/recurring/{content.Id}");

        transactionResponse.ShouldNotBeNull();
        transactionResponse.Id.ShouldBe(content.Id);
        transactionResponse.Amount.ShouldBe(updatedTransaction.Amount);
        transactionResponse.NextOccurrence.ShouldBe(updatedTransaction.FirstOccurrence, TimeSpan.FromSeconds(1));
        transactionResponse.Note.ShouldBe(updatedTransaction.Note);
        transactionResponse.UserId.ShouldBe(userId);
        transactionResponse.Wallet?.Id.ShouldBe(walletId);
        transactionResponse.Category?.Id.ShouldBe(categoryId);
        transactionResponse.Payee?.Id.ShouldBe(payeeId);
    }

    [Fact]
    public async Task When_schedule_request_is_sent_then_next_occurrence_date_should_be_updated()
    {
        var userId = await _fixture.GetRandomUser();
        var walletId = await _fixture.GetRandomWallet(userId);
        var categoryId = await _fixture.GetRandomCategory(userId);
        var payeeId = await _fixture.GetRandomPayee(userId);

        var transaction = DataFaker.GenerateRecurringTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions/recurring", new { UserId = userId, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.FirstOccurrence, transaction.Note, TransactionType = 0, transaction.Schedule });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<RecurringTransactionResponse>();

        var scheduleResponse = await _client
            .PatchAsync($"transactions/recurring/{content!.Id}/schedule", null);

        scheduleResponse.IsSuccessStatusCode.ShouldBeTrue();

        var transactionResponse = await _client
            .GetFromJsonAsync<RecurringTransactionResponse>($"transactions/recurring/{content.Id}");

        transactionResponse.ShouldNotBeNull();
        transactionResponse.NextOccurrence.Date.Should().BeOnOrAfter(transaction.FirstOccurrence.Date);
    }

}
record RecurringTransactionResponse(int Id, int UserId, WalletResponse Wallet, CategoryResponse Category, PayeeResponse Payee, decimal Amount, DateTime NextOccurrence, string Note, string Schedule);
