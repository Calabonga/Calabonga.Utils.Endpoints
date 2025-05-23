using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.Utils.Endpoints.Demo.Infrastructure;

namespace Calabonga.Utils.Endpoints.Demo.Endpoints;

public sealed class ModuleEndpoint : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("/info", (ModuleService moduleService) =>
            {
                var items = moduleService.GetInformation();
                return Results.Ok(items);
            })
            .ExcludeFromDescription()
            .WithOpenApi();
    }
}
