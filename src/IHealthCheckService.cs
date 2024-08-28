namespace NHealthCheck;

/// <summary>
/// With the Pinging API, you can signal success, start, failure, and log events from your systems.
/// </summary>
public interface IHealthCheckService
{
    /// <summary>
    /// Signals to Healthchecks.io that the job has been completed successfully (or a continuously running process is still running and healthy).
    /// </summary>
    /// <param name="uuid">UUID associated with the relevant check.</param>
    /// <param name="runId">(Optional) Specifies the Run ID of this ping.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> which represents the response returned from the Healthchecks.io API.<br/>
    /// <br/>
    /// Possible values include:<br/>
    /// <list type="bullet"> 
    /// <item>200 OK - The request was understood and added to processing queue.</item>
    /// <item>200 OK (not found) - Could not find a check with the specified UUID.</item>
    /// <item>200 OK (rate limited) - Rate limit exceeded, request was ignored. Please do not ping a single check more than 5 times per minute.</item>
    /// <item>400 invalid url format - The URL does not match the expected format.</item>
    /// </list>
    /// </returns>
    Task<HttpResponseMessage> SuccessAsync(Guid uuid, Guid? runId = null);

    /// <summary>
    /// Sends a "job has started!" message to Healthchecks.io.
    /// </summary>
    /// <param name="uuid">UUID associated with the relevant check.</param>
    /// <param name="runId">(Optional) Specifies the Run ID of this ping.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> which represents the response returned from the Healthchecks.io API.<br/>
    /// <br/>
    /// Possible values include:<br/>
    /// <list type="bullet"> 
    /// <item>200 OK - The request was understood and added to processing queue.</item>
    /// <item>200 OK (not found) - Could not find a check with the specified UUID.</item>
    /// <item>200 OK (rate limited) - Rate limit exceeded, request was ignored. Please do not ping a single check more than 5 times per minute.</item>
    /// <item>400 invalid url format - The URL does not match the expected format.</item>
    /// </list>
    /// </returns>
    Task<HttpResponseMessage> StartAsync(Guid uuid, Guid? runId = null);

    /// <summary>
    /// Signals to Healthchecks.io that the job has failed.
    /// </summary>
    /// <param name="uuid">UUID associated with the relevant check.</param>
    /// <param name="runId">(Optional) Specifies the Run ID of this ping.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> which represents the response returned from the Healthchecks.io API.<br/>
    /// <br/>
    /// Possible values include:<br/>
    /// <list type="bullet"> 
    /// <item>200 OK - The request was understood and added to processing queue.</item>
    /// <item>200 OK (not found) - Could not find a check with the specified UUID.</item>
    /// <item>200 OK (rate limited) - Rate limit exceeded, request was ignored. Please do not ping a single check more than 5 times per minute.</item>
    /// <item>400 invalid url format - The URL does not match the expected format.</item>
    /// </list>
    /// </returns>
    Task<HttpResponseMessage> FailAsync(Guid uuid, Guid? runId = null);

    /// <summary>
    /// Sends logging information to Healthchecks.io without signaling success or failure.
    /// </summary>
    /// <param name="uuid">UUID associated with the relevant check.</param>
    /// <param name="logMessage">Message to log.</param>
    /// <param name="runId">(Optional) Specifies the Run ID of this ping.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> which represents the response returned from the Healthchecks.io API.<br/>
    /// <br/>
    /// Possible values include:<br/>
    /// <list type="bullet"> 
    /// <item>200 OK - The request succeeded.</item>
    /// <item>404 not found - Could not find a check with the specified UUID.</item>
    /// </list>
    /// </returns>
    Task<HttpResponseMessage> LogAsync(Guid uuid, string logMessage, Guid? runId = null);

    /// <summary>
    /// Sends a success or failure signal depending on the exit status included in the URL.
    /// </summary>
    /// <param name="uuid">UUID associated with the relevant check.</param>
    /// <param name="exitStatus">The exit status is a 0-255 integer. 0 indicates success and all other values indicate failure.</param>
    /// <param name="runId">(Optional) Specifies the Run ID of this ping.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> which represents the response returned from the Healthchecks.io API.<br/>
    /// <br/>
    /// Possible values include:<br/>
    /// <list type="bullet"> 
    /// <item>200 OK - The request was understood and added to processing queue.</item>
    /// <item>200 OK (not found) - Could not find a check with the specified UUID.</item>
    /// <item>200 OK (rate limited) - Rate limit exceeded, request was ignored. Please do not ping a single check more than 5 times per minute.</item>
    /// <item>400 invalid url format - The URL does not match the expected format.</item>
    /// </list>
    /// </returns>
    Task<HttpResponseMessage> ExitStatusAsync(Guid uuid, int exitStatus, Guid? runId = null);
}