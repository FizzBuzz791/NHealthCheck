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
        await hcs.SuccessAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
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
        await hcs.StartAsync(uuid);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }
}