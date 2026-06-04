namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraErrorResponse
{
    public string Error { get; init; } = default!;

    public string? Details { get; init; }

    public string? Code { get; init; }
}
