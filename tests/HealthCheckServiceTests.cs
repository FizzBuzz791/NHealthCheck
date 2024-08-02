using System.Net;
using RichardSzalay.MockHttp;
using Shouldly;

namespace NHealthCheck.Tests;

public class HealthCheckServiceTests
{
    private readonly string BaseUrl = "https://hc-ping.com";

    [Test]
    public void GivenAnEmptyBaseUrlThenItThrowsError()
    {
        // Arrange
        var hcs = new HealthCheckService(new MockHttpMessageHandler().ToHttpClient());

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => hcs.SetBaseUrl(string.Empty), "Cannot clear the BaseUrl.");
    }

    [Test]
    public async Task GivenASlugWhenThePingKeyIsNotSetThenItThrowsAnError()
    {
        // Arrange
        var slug = "testslug";
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());

        // Act & Assert
        await Should.ThrowAsync<NotSupportedException>(() => hcs.SuccessAsync(slug), $"Unable to use a Slug without a Ping Key. Please call SetPingKey first.");
    }

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
        //await hcs.SuccessAsync(uuid, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }

    [Test]
    public async Task GivenASlugThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var slug = "testslug";
        var pingKey = "testpingkey";
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{pingKey}/{slug}")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());
        hcs.SetPingKey(pingKey);

        // Act
        await hcs.SuccessAsync(slug);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }

    [Test]
    public async Task GivenASlugAndARunIdThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var slug = "testslug";
        var pingKey = "testpingkey";
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{pingKey}/{slug}")
            .WithQueryString("rid", runId.ToString())
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());
        hcs.SetPingKey(pingKey);

        // Act
        await hcs.SuccessAsync(slug, runId);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }

    [Test]
    public async Task GivenASlugAndACreateFlagThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var slug = "testslug";
        var pingKey = "testpingkey";
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{pingKey}/{slug}")
            .WithQueryString("create", "1")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());
        hcs.SetPingKey(pingKey);

        // Act
        await hcs.SuccessAsync(slug, null, true);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }

    [Test]
    public async Task GivenASlugAndARunIdAndACreateFlagThenTheExpectedEndpointIsCalled()
    {
        // Arrange
        var slug = "testslug";
        var pingKey = "testpingkey";
        var runId = Guid.NewGuid();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var mockRequest = mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseUrl}/{pingKey}/{slug}")
            .WithQueryString("rid", runId.ToString())
            .WithQueryString("create", "1")
            .Respond(HttpStatusCode.OK);
        var hcs = new HealthCheckService(mockHttpMessageHandler.ToHttpClient());
        hcs.SetPingKey(pingKey);

        // Act
        await hcs.SuccessAsync(slug, runId, true);

        // Assert
        mockHttpMessageHandler.GetMatchCount(mockRequest).ShouldBe(1);
    }
}