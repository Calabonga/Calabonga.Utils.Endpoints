namespace Calabonga.Utils.Endpoints;

public record EndpointInfo(
    string? Name,
    string? RoutePattern,
    string? GroupName,
    string? Summary,
    string? Description,
    HttpMethodData? HttpMethodInfos,
    bool IsAllowAnonymous,
    AuthorizeData? AuthorizeInfo,
    OpenApiData? OpenApiData,
    IReadOnlyList<string>? Tags);
