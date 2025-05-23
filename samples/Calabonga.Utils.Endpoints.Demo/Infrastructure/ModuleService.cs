using Calabonga.OperationResults;

namespace Calabonga.Utils.Endpoints.Demo.Infrastructure;

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
