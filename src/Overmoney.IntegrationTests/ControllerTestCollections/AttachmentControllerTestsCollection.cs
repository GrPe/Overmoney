using Overmoney.IntegrationTests.Configurations;
using Overmoney.IntegrationTests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class AttachmentControllerTestsCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;
    readonly UserContext _userContext;

    public AttachmentControllerTestsCollection(InfrastructureFixture fixture)
    {
        _fixture = fixture;
        _userContext = _fixture.GetRandomUser();
        _client = fixture.GetClientForUser(_userContext);
    }

    [Fact]
    public async Task When_correct_data_are_provided_attachment_should_be_added()
    {
        var walletId = await _fixture.GetRandomWallet(_userContext);
        var categoryId = await _fixture.GetRandomCategory(_userContext);
        var payeeId = await _fixture.GetRandomPayee(_userContext);

        var transaction = DataFaker.GenerateTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = _userContext.Id, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        var attachment = DataFaker.GenerateAttachment();

        var attachmentResponse = await _client
            .PostAsJsonAsync($"transactions/attachments", new { TransactionId = content.Id, attachment.Path, attachment.Name });

        attachmentResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task When_attachment_is_added_and_update_request_is_sent_then_attachment_should_be_updated()
    {
        var walletId = await _fixture.GetRandomWallet(_userContext);
        var categoryId = await _fixture.GetRandomCategory(_userContext);
        var payeeId = await _fixture.GetRandomPayee(_userContext);

        var transaction = DataFaker.GenerateTransaction();
        var response = await _client
            .PostAsJsonAsync("transactions", new { UserId = _userContext.Id, WalletId = walletId, CategoryId = categoryId, PayeeId = payeeId, transaction.Amount, transaction.TransactionDate, transaction.Note, TransactionType = 0 });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        var attachment = DataFaker.GenerateAttachment();

        var attachmentResponse = await _client
            .PostAsJsonAsync($"transactions/attachments", new { TransactionId = content.Id, attachment.Path, attachment.Name });

        attachmentResponse.IsSuccessStatusCode.ShouldBeTrue();

        var attachmentContent = await attachmentResponse.Content.ReadFromJsonAsync<AttachmentResponse>();

        var updatedAttachment = DataFaker.GenerateAttachment();

        var updateResponse = await _client
            .PatchAsJsonAsync($"attachments", new { attachmentContent!.Id, updatedAttachment.Name });

        updateResponse.IsSuccessStatusCode.ShouldBeTrue();
    }
}

public record AttachmentResponse(long Id, string Name, string Path);