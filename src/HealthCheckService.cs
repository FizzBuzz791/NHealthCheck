using System.Net;
using NHealthCheck.Helpers;

namespace NHealthCheck;

/// <inheritdoc />
public class HealthCheckService : IHealthCheckService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    private readonly HttpClient HttpClient;

    private string BaseUrl { get; set; } = "https://hc-ping.com";

    public HealthCheckService(HttpClient httpClient)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<HttpResponseMessage> SuccessAsync(Guid uuid, Guid? runId = null)
    {
        var url = $"{BaseUrl}/{uuid}";

        if (runId.HasValue)
        {
            var qpb = new QueryParamBuilder();
            qpb.AddQueryParam("rid", runId.Value.ToString());

            url += qpb.ToQueryString();
        }

        return await CallEndpoint(url);
    }

    public async Task<HttpResponseMessage> StartAsync(Guid uuid, Guid? runId = null)
    {
        var url = $"{BaseUrl}/{uuid}/start";

        if (runId.HasValue)
        {
            var qpb = new QueryParamBuilder();
            qpb.AddQueryParam("rid", runId.Value.ToString());

            url += qpb.ToQueryString();
        }

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