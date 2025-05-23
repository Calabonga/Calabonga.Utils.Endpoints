using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace Calabonga.Utils.Endpoints;
/// <summary>
/// Extenstion for <see cref="Endpoint"/>
/// </summary>
public static class EndpointExtensions
{
    public static bool GetEndpointExcluded(this Endpoint source)
    {
        var data = source.Metadata.GetMetadata<ExcludeFromDescriptionAttribute>();
        return data?.ExcludeFromDescription ?? false;
    }

    public static OpenApiData? GetOpenApiInfo(this Endpoint source)
    {
        var data = source.Metadata.GetMetadata<OpenApiOperation>();
        if (data is null)
        {
            return null;
        }

        var description = data.Description;
        var summary = data.Summary;
        var operationId = data.OperationId;

        return new OpenApiData(description, summary, operationId);
    }

    public static AuthorizeData? GetAuthorizeInfo(this Endpoint source)
    {
        var data = source.Metadata.GetMetadata<AuthorizeAttribute>();
        if (data is null)
        {
            return null;
        }

        var policy = data.Policy;
        var authenticationSchemes = data.AuthenticationSchemes;
        var roles = data.Roles;

        return new AuthorizeData(policy, authenticationSchemes, roles);
    }

    public static bool GetAnonymousAllowed(this Endpoint source)
    {
        return source.Metadata.GetMetadata<AllowAnonymousAttribute>() is not null;
    }

    public static HttpMethodData? GetHttpMethodsInfo(this Endpoint source)
    {
        var data = source.Metadata.GetMetadata<HttpMethodMetadata>();
        return data is null
            ? null
            : new HttpMethodData(data.HttpMethods, data.AcceptCorsPreflight);
    }

    public static IReadOnlyList<string>? GetTagsInfo(this Endpoint source)
    {
        var data = source.Metadata.GetMetadata<TagsAttribute>();
        return data?.Tags;
    }
    public static string? GetEndpointSummary(this Endpoint source)
    {
        return source.Metadata.GetMetadata<EndpointSummaryAttribute>()?.Summary;
    }

    public static string? GetEndpointName(this Endpoint source)
    {
        return source.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
    }

    public static string? GetDescription(this Endpoint source)
    {
        return source.Metadata.GetMetadata<EndpointDescriptionAttribute>()?.Description;
    }

    public static string? GetGroupName(this Endpoint source)
    {
        return source.Metadata.GetMetadata<EndpointGroupNameAttribute>()?.EndpointGroupName;
    }

    public static string? GetRoutePatternAsRawText(this Endpoint source)
    {
        return ((RouteEndpoint)source).RoutePattern.RawText;
    }
}
