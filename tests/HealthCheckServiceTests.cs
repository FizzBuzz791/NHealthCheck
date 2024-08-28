using System.Net;
using RichardSzalay.MockHttp;
using Shouldly;

namespace NHealthCheck.Tests;

public class HealthCheckServiceTests
{
    private readonly string BaseUrl = "https://hc-ping.com";

    [Test]
    public async Task GivenAnUUIDThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.SuccessAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDAndARunIdThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.SuccessAsync(uuid, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDWhenTimingAProcessThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/start")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.StartAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDAndARunIdWhenTimingAProcessThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/start")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.StartAsync(uuid, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenABadRequestWhenAnExceptionIsThrownThenInternalServerErrorIsReturned()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}")
            .Throw(new Exception("Test Exception"));
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.SuccessAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        response.ReasonPhrase.ShouldBe("Test Exception");
    }

    [Test]
    public async Task GivenAnUUIDWhenReportingAFailThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/fail")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.FailAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDAndARunIdWhenReportingAFailThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/fail")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.FailAsync(uuid, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDWhenLoggingThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Post, $"{BaseUrl}/{uuid}/log")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.LogAsync(uuid, "Hello, World!");

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDAndARunIdWhenLoggingThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Post, $"{BaseUrl}/{uuid}/log")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.LogAsync(uuid, "Hello, World!", runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDWhenReportingAnExitCodeThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var exitCode = 0;
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/{exitCode}")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.ExitStatusAsync(uuid, exitCode);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenAnUUIDAndARunIdWhenReportingAnExitCodeThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var exitCode = 0;
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{uuid}/{exitCode}")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act
        var response = await hcs.ExitStatusAsync(uuid, exitCode, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}