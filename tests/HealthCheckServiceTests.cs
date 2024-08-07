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
}