namespace Calabonga.Utils.Endpoints;

public record AuthorizeData(
    string? Policy,
    string? AuthenticationSchemes,
    string? Roles);
