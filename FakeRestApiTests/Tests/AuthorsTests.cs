using FakeRestApiTests.Helpers;
using FakeRestApiTests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FakeRestApiTests.Tests;

public class AuthorsTests : IDisposable
{
    private readonly ApiClient _apiClient;
    private readonly ApiTestLogger _logger;

    public AuthorsTests(ITestOutputHelper output)
    {
        _apiClient = new ApiClient();
        _logger = new ApiTestLogger(output);
    }

    [Fact]
    public async Task PostAndDeleteAuthor_Should_WorkCorrectly()
    {
        var endpoint = "/api/v1/Authors";
        var author = new Author { IdBook = 1, FirstName = "John", LastName = "Doe" };
        var requestBody = JsonConvert.SerializeObject(author);

        var (statusCode, created, responseBody) = await _apiClient.PostAsync<Author>(endpoint, author);

        try
        {
            statusCode.Should().Be(200);
            created.Should().NotBeNull();
            created!.IdBook.Should().Be(author.IdBook);
            created.FirstName.Should().Be(author.FirstName);
            created.LastName.Should().Be(author.LastName);
        }
        catch (Exception)
        {
            _logger.LogAssertionFailure(endpoint, requestBody, statusCode, responseBody, requestBody, responseBody);
            throw;
        }

        var deleteEndpoint = $"/api/v1/Authors/{created.Id}";
        var (deleteStatus, deleteResponse) = await _apiClient.DeleteAsync(deleteEndpoint);

        try
        {
            deleteStatus.Should().Be(200);
        }
        catch (Exception)
        {
            _logger.LogAssertionFailure(deleteEndpoint, null, deleteStatus, deleteResponse, "200", deleteStatus.ToString());
            throw;
        }
    }

    public void Dispose()
    {
        _apiClient?.Dispose();
    }
}
