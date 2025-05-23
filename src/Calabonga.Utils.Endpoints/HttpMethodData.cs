namespace Calabonga.Utils.Endpoints;

public record HttpMethodData(
    IReadOnlyList<string>? HttpMethods,
    bool AcceptCorsPreflight);
