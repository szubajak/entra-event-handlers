namespace Entra.EventHandlers.Abstractions.Errors;

public sealed class EntraErrorResponse
{
    public required string Error { get; init; }

    public string? Details { get; init; }
}
