using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.Utils.Endpoints.Demo.Infrastructure;

namespace Calabonga.Utils.Endpoints.Demo.Definitions;

public sealed class DependencyContainerDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ModuleService>();
    }
}
