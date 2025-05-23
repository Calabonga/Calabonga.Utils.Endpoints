using Calabonga.AspNetCore.AppDefinitions;

namespace Calabonga.Utils.Endpoints.Demo.Definitions;
/// <summary>
/// Common definition
/// </summary>
public class CommonDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi("v1");
        builder.Services.AddOpenApi("v2");


    }

    public override void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", $"{app.Environment.ApplicationName} v1");
                options.SwaggerEndpoint("/openapi/v2.json", $"{app.Environment.ApplicationName} v2");
            });
        }

        app.UseHttpsRedirection();
    }
}
