namespace NHealthCheck;

public interface IHealthCheckService
{
    /**
     * Signals to Healthchecks.io that the job has been completed successfully (or a continuously running process is still running and healthy).
     */
    Task<HttpResponseMessage> SuccessAsync(Guid uuid);

    /**
     * Sends a "job has started!" message to Healthchecks.io.
     */
    Task<HttpResponseMessage> StartAsync(Guid uuid);
}