using NHealthCheck.Helpers;

namespace NHealthCheck;

public class HealthCheckService(HttpClient httpClient) : IHealthCheckService
{
    private readonly HttpClient HttpClient = httpClient;

    private string BaseUrl { get; set; } = "https://hc-ping.com";

    public async Task SuccessAsync(Guid uuid)
    {
        var url = $"{BaseUrl}/{uuid}";
        await CallEndpoint(url);
    }

    public async Task StartAsync(Guid uuid)
    {
        var url = $"{BaseUrl}/{uuid}/start";
        await CallEndpoint(url);
    }

    private async Task CallEndpoint(string url)
    {
        try
        {
            HttpClient.BaseAddress = new Uri(BaseUrl);
            await HttpClient.GetAsync(url);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error pinging Health Check endpoint: {ex.Message}");
        }
    }
}