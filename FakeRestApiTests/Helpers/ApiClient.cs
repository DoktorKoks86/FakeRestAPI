using System.Text;
using Newtonsoft.Json;

namespace FakeRestApiTests.Helpers;

public class ApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://fakerestapi.azurewebsites.net";

    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<(int StatusCode, T? Data, string ResponseBody)> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        var responseBody = await response.Content.ReadAsStringAsync();
        var data = response.IsSuccessStatusCode 
            ? JsonConvert.DeserializeObject<T>(responseBody) 
            : default;
        
        return ((int)response.StatusCode, data, responseBody);
    }

    public async Task<(int StatusCode, T? Data, string ResponseBody)> PostAsync<T>(string endpoint, object requestBody)
    {
        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var data = response.IsSuccessStatusCode 
            ? JsonConvert.DeserializeObject<T>(responseBody) 
            : default;
        
        return ((int)response.StatusCode, data, responseBody);
    }

    public async Task<(int StatusCode, T? Data, string ResponseBody)> PutAsync<T>(string endpoint, object requestBody)
    {
        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(endpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var data = response.IsSuccessStatusCode 
            ? JsonConvert.DeserializeObject<T>(responseBody) 
            : default;
        
        return ((int)response.StatusCode, data, responseBody);
    }

    public async Task<(int StatusCode, string ResponseBody)> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        return ((int)response.StatusCode, responseBody);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
