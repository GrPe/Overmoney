using Overmoney.IntegrationTests.Configurations;
using Overmoney.IntegrationTests.Models;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTestCollections;

[Collection("Infrastructure")]
public class CategoryControllerTestCollection
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;
    readonly UserContext _userContext;

    public CategoryControllerTestCollection(InfrastructureFixture fixture)
    {
        _fixture = fixture;
        _userContext = _fixture.GetRandomUser();
        _client = fixture.GetClientForUser(_userContext);
    }

    [Fact]
    public async Task When_correct_data_are_provided_category_should_be_created()
    {
        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_api_should_return_bad_request()
    {
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = "" });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_category_exists_and_update_request_is_sent_then_category_should_be_updated()
    {
        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        var updatedCategory = DataFaker.GenerateCategory();
        var putResponse = await _client
            .PutAsJsonAsync($"categories", new { content!.Id, UserId = _userContext.Id, Name = updatedCategory });

        putResponse.IsSuccessStatusCode.ShouldBeTrue();

        var updatedContent = await _client
            .GetFromJsonAsync<CategoryResponse>($"categories/{content.Id}");

        updatedContent.ShouldNotBeNull();
        updatedContent.Id.ShouldBe(content.Id);
        updatedContent.Name.ShouldBe(updatedCategory);
    }

    [Fact]
    public async Task When_categories_exists_api_should_return_a_list_of_categories()
    {
        var category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        var categories = await _client
            .GetFromJsonAsync<List<CategoryResponse>>($"users/{_userContext.Id}/categories");

        categories.ShouldNotBeNull();
        categories.Count.ShouldBeGreaterThan(2);
    }

    [Fact]
    public async Task When_category_exists_and_delete_request_is_sent_then_category_should_be_deleted()
    {
        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = _userContext.Id, Name = category });

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        var deleteResponse = await _client
            .DeleteAsync($"categories/{content!.Id}");

        deleteResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

}

record CategoryResponse(int Id, string Name);