using FakeRestApiTests.Helpers;
using FakeRestApiTests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace FakeRestApiTests.Tests;

public class ActivitiesTests : IDisposable
{
    private readonly ApiClient _apiClient;
    private readonly ApiTestLogger _logger;

    public ActivitiesTests(ITestOutputHelper output)
    {
        _apiClient = new ApiClient();
        _logger = new ApiTestLogger(output);
    }

    [Fact]
    public async Task GetActivities_Should_ReturnValidData()
    {
        var endpoint = "/api/v1/Activities";
        var yesterday = DateTime.Now.Date.AddDays(-1);

        var (statusCode, activities, responseBody) = await _apiClient.GetAsync<List<Activity>>(endpoint);

        try
        {
            statusCode.Should().Be(200);
            activities.Should().NotBeNull();
            activities!.Count.Should().Be(30);
            activities.Where(a => a.DueDate.Date == yesterday).Should().BeEmpty();
        }
        catch (Exception)
        {
            _logger.LogAssertionFailure(endpoint, null, statusCode, responseBody, 
                "Status: 200, Count: 30, No yesterday dates", 
                $"Status: {statusCode}, Count: {activities?.Count}, Yesterday count: {activities?.Count(a => a.DueDate.Date == yesterday)}");
            throw;
        }
    }

    public void Dispose()
    {
        _apiClient?.Dispose();
    }
}
