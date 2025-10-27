using FakeRestApiTests.Helpers;
using FakeRestApiTests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FakeRestApiTests.Tests;

public class BooksTests : IDisposable
{
    private readonly ApiClient _apiClient;
    private readonly ApiTestLogger _logger;

    public BooksTests(ITestOutputHelper output)
    {
        _apiClient = new ApiClient();
        _logger = new ApiTestLogger(output);
    }

    [Fact]
    public async Task PutBook_Should_ReturnSameData()
    {
        var bookId = 5;
        var endpoint = $"/api/v1/Books/{bookId}";
        var book = new Book
        {
            Id = bookId,
            Title = "Test Book",
            Description = "Test Description",
            PageCount = 500,
            Excerpt = "Test Excerpt",
            PublishDate = DateTime.Now
        };
        var requestBody = JsonConvert.SerializeObject(book);

        var (statusCode, response, responseBody) = await _apiClient.PutAsync<Book>(endpoint, book);

        try
        {
            statusCode.Should().Be(200);
            response.Should().NotBeNull();
            response!.Id.Should().Be(book.Id);
            response.Title.Should().Be(book.Title);
            response.Description.Should().Be(book.Description);
            response.PageCount.Should().Be(book.PageCount);
            response.Excerpt.Should().Be(book.Excerpt);
        }
        catch (Exception)
        {
            _logger.LogAssertionFailure(endpoint, requestBody, statusCode, responseBody, requestBody, responseBody);
            throw;
        }
    }

    // Testing pattern: PageCount = bookId * 100 (100, 200, 300... 1000)
    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 200)]
    [InlineData(3, 300)]
    [InlineData(4, 400)]
    [InlineData(5, 500)]
    [InlineData(6, 600)]
    [InlineData(7, 700)]
    [InlineData(8, 800)]
    [InlineData(9, 900)]
    [InlineData(10, 1000)]
    public async Task GetBook_Should_ReturnCorrectData(int bookId, int expectedPageCount)
    {
        var endpoint = $"/api/v1/Books/{bookId}";
        var (statusCode, book, responseBody) = await _apiClient.GetAsync<Book>(endpoint);

        try
        {
            statusCode.Should().Be(200);
            book.Should().NotBeNull();
            book!.Id.Should().Be(bookId);
            book.PageCount.Should().Be(expectedPageCount);
        }
        catch (Exception)
        {
            _logger.LogAssertionFailure(endpoint, null, statusCode, responseBody,
                $"Id={bookId}, PageCount={expectedPageCount}", 
                $"Id={book?.Id}, PageCount={book?.PageCount}");
            throw;
        }
    }

    public void Dispose()
    {
        _apiClient?.Dispose();
    }
}
