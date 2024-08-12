# NHealthCheck

[![CI/CD](https://github.com/FizzBuzz791/NHealthCheck/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FizzBuzz791/NHealthCheck/actions/workflows/dotnet.yml)
[![Coverage Status](https://coveralls.io/repos/github/FizzBuzz791/NHealthCheck/badge.svg?branch=main)](https://coveralls.io/github/FizzBuzz791/NHealthCheck?branch=main)

.NET implementation of the APIs available at [Healthchecks.io](https://healthchecks.io)

## Installation

### CLI

`dotnet add package NHealthCheck`

### NuGet

Search for `NHealthCheck` in your package manager of choice.

## Usage

This package is built with dependency injection in mind. It's recommended to register it first (Native):

```csharp
builder.Services.AddHttpClient<IHealthCheckService, HealthCheckService>();
```

or (Autofac):

```csharp
var services = new ServiceCollection();
services.AddHttpClient<IHealthCheckService, HealthCheckService>();
builder.Populate(services);
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
