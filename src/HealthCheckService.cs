using System.Net;
using System.Net.Http.Headers;
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

        return await CallEndpoint(new HttpRequestMessage(HttpMethod.Get, url));
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

        return await CallEndpoint(new HttpRequestMessage(HttpMethod.Get, url));
    }

    public async Task<HttpResponseMessage> FailAsync(Guid uuid, Guid? runId = null)
    {
        var url = $"{BaseUrl}/{uuid}/fail";

        if (runId.HasValue)
        {
            var qpb = new QueryParamBuilder();
            qpb.AddQueryParam("rid", runId.Value.ToString());

            url += qpb.ToQueryString();
        }

        return await CallEndpoint(new HttpRequestMessage(HttpMethod.Get, url));
    }

    public async Task<HttpResponseMessage> LogAsync(Guid uuid, string logMessage, Guid? runId = null)
    {
        var url = $"{BaseUrl}/{uuid}/log";

        if (runId.HasValue)
        {
            var qpb = new QueryParamBuilder();
            qpb.AddQueryParam("rid", runId.Value.ToString());

            url += qpb.ToQueryString();
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(logMessage, new MediaTypeHeaderValue("text/plain"))
        };

        return await CallEndpoint(request);
    }

    private async Task<HttpResponseMessage> CallEndpoint(HttpRequestMessage requestMessage)
    {
        try
        {
            return await HttpClient.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error pinging Health Check endpoint: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = ex.Message };
        }
    }
}