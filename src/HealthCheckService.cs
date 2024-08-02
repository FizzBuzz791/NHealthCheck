using NHealthCheck.Helpers;

namespace NHealthCheck;

public class HealthCheckService(HttpClient httpClient) : IHealthCheckService
{
    private readonly HttpClient HttpClient = httpClient;

    private string BaseUrl { get; set; } = "https://hc-ping.com";
    private string? PingKey { get; set; } = null;

    public void SetBaseUrl(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentNullException(nameof(baseUrl), "Cannot clear the BaseUrl.");
        }

        BaseUrl = baseUrl;
    }

    public void SetPingKey(string? pingKey)
    {
        PingKey = pingKey;
    }

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

    public async Task SuccessAsync(string slug, Guid? runId = null, bool? create = null)
    {
        if (string.IsNullOrWhiteSpace(PingKey))
        {
            throw new NotSupportedException($"Unable to use a Slug without a Ping Key. Please call {nameof(SetPingKey)} first.");
        }

        var url = $"{BaseUrl}/{PingKey}/{slug}";
        var qpb = new QueryParamBuilder();

        if (runId.HasValue)
        {
            qpb.AddQueryParam("rid", runId.Value.ToString());
        }

        if (create.HasValue)
        {
            qpb.AddQueryParam("create", create.Value ? "1" : "0");
        }

        url += qpb.ToQueryString();

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