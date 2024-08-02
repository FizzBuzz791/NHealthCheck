# NHealthCheck

.NET implementation of the APIs available at [Healthchecks.io](https://healthchecks.io)

## Installation

### CLI

`dotnet add package NHealthCheck

### NuGet

Search for `NHealthCheck` in your package manager of choice.

## Usage

This package is built with dependency injection in mind. It's recommended to register it first (Autofac):

```csharp
builder.Services.AddHttpClient<IHealthCheckService, HealthCheckService>();
```

The simplest use is to call the success method when your job finishes:

```csharp
public class MyJob
{
    private IHeathCheckService HealthCheckService { get; }

    public MyJob(IHealthCheckService healthCheckService)
    {
        HealthCheckService = healthCheckService;
    } 

    public Task DoWork()
    {
        ...
        
        await HealthCheckService.SuccessAsync(new Guid(config.HealthCheckGuid));
    }
}
```