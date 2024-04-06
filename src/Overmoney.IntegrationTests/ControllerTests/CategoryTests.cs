using Overmoney.IntegrationTests.Configurations;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace Overmoney.IntegrationTests.ControllerTests;

[Collection("Infrastructure")]
public class CategoryTests
{
    readonly HttpClient _client;
    readonly InfrastructureFixture _fixture;

    public CategoryTests(InfrastructureFixture fixture)
    {
        _client = fixture.GetClient();
        _fixture = fixture;
    }

    [Fact]
    public async Task When_correct_data_are_provided_category_should_be_created()
    {
        var userId = await _fixture.GetRandomUser();

        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        response.IsSuccessStatusCode.ShouldBeTrue();

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        content.ShouldNotBeNull();
        content.Id.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task When_incorrect_data_are_provided_api_should_return_bad_request()
    {
        var userId = await _fixture.GetRandomUser();

        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = "" });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_category_exists_and_update_request_is_sent_then_category_should_be_updated()
    {
        var userId = await _fixture.GetRandomUser();

        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        var updatedCategory = DataFaker.GenerateCategory();
        var putResponse = await _client
            .PutAsJsonAsync($"categories", new { content!.Id, UserId = userId, Name = updatedCategory });

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
        var userId = await _fixture.GetRandomUser();

        var category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        category = DataFaker.GenerateCategory();
        await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        var categories = await _client
            .GetFromJsonAsync<List<CategoryResponse>>($"users/{userId}/categories");

        categories.ShouldNotBeNull();
        categories.Count.ShouldBeGreaterThan(2);
    }

    [Fact]
    public async Task When_category_exists_and_delete_request_is_sent_then_category_should_be_deleted()
    {
        var userId = await _fixture.GetRandomUser();

        var category = DataFaker.GenerateCategory();
        var response = await _client
            .PostAsJsonAsync("categories", new { UserId = userId, Name = category });

        var content = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        var deleteResponse = await _client
            .DeleteAsync($"categories/{content!.Id}");

        deleteResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

}

file record CategoryResponse(int Id, string Name);