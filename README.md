# Extensions for Endpoints (ASP.NET Core)

Simple but very helpful library that's allow to get all metadata for minimal API endpoints using OpenAPI specification.

## Версия 1.0.0

* First release.

## How to use

Imagine you have a minimal API with some methods (something like shown below):

```csharp
public override void ConfigureApplication(WebApplication app)
{
    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    
    var group = app.MapGroup("/weather-forecast");

    group.MapGet("/secured", ([FromServices] ILogger<WeatherForecastEndpoints> logger) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();
            logger.LogInformation("WeatherForecast request execute at [{Time}].", DateTime.UtcNow);
            return forecast;
        })
        .WithName("GetWeatherForecastSecured")
        .WithSummary("This is summary for WeatherForecast Method1")
        .RequireAuthorization("CalabongaPolicy")
        .ProducesValidationProblem(400, "application/problem+json")
        .ProducesProblem(400, "application/problem+json")
        .ProducesProblem(401, "application/problem+json")
        .WithDescription("CalabongaDescription1")
        .WithDisplayName("This is a DisplayName With for Secured")
        .WithTags("Weather", "Forecast")
        .WithOpenApi();

    group.MapGet("/anonymous", ([FromServices] ILogger<WeatherForecastEndpoints> logger) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();
            logger.LogInformation("WeatherForecast request execute at [{Time}].", DateTime.UtcNow);
            return forecast;
        })
        .WithName("GetWeatherForecastForAll")
        .WithSummary("This is summary for WeatherForecast Method2")
        .AllowAnonymous()
        .ProducesValidationProblem(400, "application/problem+json")
        .ProducesProblem(400, "application/problem+json")
        .ProducesProblem(401, "application/problem+json")
        .WithDescription("CalabongaDescription2")
        .WithDisplayName("This is a DisplayName With for Anonymous")
        .WithTags("Weather", "Forecast", "Insecure")
        .WithOpenApi();
}
```

When you want to see a metadata for each `Endpoint` you can use current nuget. Something like this:

```csharp
public class ModuleService
{
    private readonly IEnumerable<EndpointDataSource> _endpointData;

    public ModuleService(IEnumerable<EndpointDataSource> endpointData)
    {
        _endpointData = endpointData;
    }

    public Operation<IReadOnlyList<EndpointInfo>, string> GetInformation()
    {
        var endpoints = _endpointData.SelectMany(x => x.Endpoints).ToList();
        if (!endpoints.Any())
        {
            return Operation.Error("No endpoints were found");
        }

        var result = new List<EndpointInfo>();
        foreach (var endpoint in endpoints)
        {
            var isExcluded = endpoint.GetEndpointExcluded();
            if (isExcluded)
            {
                continue;
            }

            var item = new EndpointInfo
            (
                endpoint.GetEndpointName(),
                endpoint.GetRoutePatternAsRawText(),
                endpoint.GetGroupName(),
                endpoint.GetEndpointSummary(),
                endpoint.GetDescription(),
                endpoint.GetHttpMethodsInfo(),
                endpoint.GetAnonymousAllowed(),
                endpoint.GetAuthorizeInfo(),
                endpoint.GetOpenApiInfo(),
                endpoint.GetTagsInfo()
            );

            result.Add(item);
        }

        return result;
    }
}
```

Metadata from Endpoints will look's like that:
```json
{
  "result": [
    {
      "name": "GetWeatherForecastSecured",
      "routePattern": "/weather-forecast/secured",
      "groupName": null,
      "summary": "This is summary for WeatherForecast Method1",
      "description": "CalabongaDescription1",
      "httpMethodInfos": {
        "httpMethods": [
          "GET"
        ],
        "acceptCorsPreflight": false
      },
      "isAllowAnonymous": false,
      "authorizeInfo": {
        "policy": "CalabongaPolicy",
        "authenticationSchemes": null,
        "roles": null
      },
      "openApiData": {
        "description": "CalabongaDescription1",
        "summary": "This is summary for WeatherForecast Method1",
        "operationId": "GetWeatherForecastSecured"
      },
      "tags": [
        "Weather",
        "Forecast"
      ]
    },
    {
      "name": "GetWeatherForecastForAll",
      "routePattern": "/weather-forecast/anonymous",
      "groupName": null,
      "summary": "This is summary for WeatherForecast Method2",
      "description": "CalabongaDescription2",
      "httpMethodInfos": {
        "httpMethods": [
          "GET"
        ],
        "acceptCorsPreflight": false
      },
      "isAllowAnonymous": true,
      "authorizeInfo": null,
      "openApiData": {
        "description": "CalabongaDescription2",
        "summary": "This is summary for WeatherForecast Method2",
        "operationId": "GetWeatherForecastForAll"
      },
      "tags": [
        "Weather",
        "Forecast",
        "Insecure"
      ]
    }
  ],
  "ok": true
}
```