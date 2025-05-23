using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.Utils.Endpoints.Demo.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Calabonga.Utils.Endpoints.Demo.Endpoints;
/// <summary>
/// Endpoint definition for <see cref="WeatherForecast"/> entity.
/// </summary>
public sealed class WeatherForecastEndpoints : AppDefinition
{
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
}
