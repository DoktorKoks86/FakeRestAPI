using Xunit.Abstractions;

namespace FakeRestApiTests.Helpers;

public class ApiTestLogger
{
    private readonly ITestOutputHelper _output;

    public ApiTestLogger(ITestOutputHelper output)
    {
        _output = output;
    }

    public void LogAssertionFailure(
        string endpoint,
        string? requestBody,
        int statusCode,
        string? responseBody,
        string expectedValue,
        string actualValue)
    {
        _output.WriteLine($"\nEndpoint: {endpoint}");
        _output.WriteLine($"Status: {statusCode}");
        if (requestBody != null) _output.WriteLine($"Request: {requestBody}");
        if (responseBody != null) _output.WriteLine($"Response: {responseBody}");
        _output.WriteLine($"Expected: {expectedValue}");
        _output.WriteLine($"Actual: {actualValue}\n");
    }
}
