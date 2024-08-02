namespace NHealthCheck;

public interface IHealthCheckService
{
    /**
     * Used to override the default (https://hc-ping.com)
     */
    void SetBaseUrl(string baseUrl);

    /**
     * Used to override the default (null)
     */
    void SetPingKey(string pingKey);

    /**
     * Signals to Healthchecks.io that the job has been completed successfully (or a continuously running process is still running and healthy).
     */
    Task SuccessAsync(Guid uuid);

    /**
     * Sends a "job has started!" message to Healthchecks.io.
     */
    Task StartAsync(Guid uuid);

    /**
     * Basic ping to indicate the service successfully ran to completion.
     */
    Task SuccessAsync(string slug, Guid? runId = null, bool? create = null);
}