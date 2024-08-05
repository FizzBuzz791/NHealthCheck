using System.Net;

namespace NHealthCheck;

public class HealthCheckService : IHealthCheckService
{
    private readonly HttpClient HttpClient;

    private string BaseUrl { get; set; } = "https://hc-ping.com";

    public HealthCheckService(HttpClient httpClient)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<HttpResponseMessage> SuccessAsync(Guid uuid)
    {
        var url = $"{BaseUrl}/{uuid}";
        return await CallEndpoint(url);
    }

    public async Task<HttpResponseMessage> StartAsync(Guid uuid)
    {
        var url = $"{BaseUrl}/{uuid}/start";
        return await CallEndpoint(url);
    }

    private async Task<HttpResponseMessage> CallEndpoint(string url)
    {
        try
        {
            return await HttpClient.GetAsync(url);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error pinging Health Check endpoint: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = ex.Message };
        }
    }
}