using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Builders;
using Entra.EventHandlers.Interfaces;
using Entra.EventHandlers.Protocol.PasswordSubmit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Entra.EventHandlers.Handlers.Base;

/// <summary>
/// Provides a base implementation of <see cref="IPasswordSubmitHandler"/>
/// that applies shared processing behavior such as structured logging,
/// correlation scoping, execution timing, and exception handling.
/// Derived classes should override <see cref="HandleCoreAsync"/> to implement
/// event‑specific business logic for the PasswordSubmit flow.
/// This base class also validates incoming events according to the Entra
/// protocol contract before invoking handler logic.
/// </summary>
public abstract class PasswordSubmitHandlerBase(ILogger logger, IPasswordContextCryptoService cryptoService) : IPasswordSubmitHandler
{
    protected ILogger Logger { get; } = logger;
    protected IPasswordContextCryptoService CryptoService { get; } = cryptoService;

    /// <summary>
    /// Handles the PasswordSubmit event using a standardized processing
    /// pipeline. This includes protocol‑level validation, correlation‑scoped
    /// logging, execution timing, and consistent exception handling.
    /// </summary>
    /// <remarks>
    /// This method performs validation of the incoming event (including
    /// <c>@odata.type</c> verification), establishes a logging scope with
    /// correlation identifiers, measures execution duration, and ensures
    /// that unhandled exceptions result in a safe <c>Block</c> response.
    /// </remarks>
    public async Task<PasswordSubmitResponse> HandleAsync(PasswordSubmitEvent request, CancellationToken cancellationToken)
    {
        using var scope = Logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["EventType"] = request.Type,
            ["EventName"] = request.GetType().Name
        });

        var sw = Stopwatch.StartNew();

        Logger.LogInformation("Handling event");

        DecryptedPasswordContext? decrypted = null;

        try
        {
            request.Validate();

            decrypted = CryptoService.Decrypt(request.Data.EncryptedPasswordContext);

            var response = await HandleCoreAsync(request, decrypted, cancellationToken);

            sw.Stop();

            var actionTypes = response.Data.Actions.Any()
                ? string.Join(",", response.Data.Actions.Select(a => a.OdataType))
                : "None";

            Logger.LogInformation(
                "Successfully handled event. DurationMs={Duration}, Actions={ActionTypes}",
                sw.ElapsedMilliseconds,
                actionTypes);

            return response!;
        }
        catch (Exception ex)
        {
            sw.Stop();

            Logger.LogError(
                ex,
                "Unhandled exception. DurationMs={Duration}",
                sw.ElapsedMilliseconds);

            if (decrypted?.Nonce is string nonce)
            {
                return EntraEventResponses
                    .PasswordSubmit()
                    .WithNonce(nonce)
                    .Block()
                    .Build();
            }

            throw;
        }
    }

    /// <summary>
    /// Contains the event‑specific business logic for handling the
    /// PasswordSubmit event. Implementations should override this method
    /// instead of <see cref="HandleAsync"/>.
    /// </summary>
    /// <remarks>
    /// This method receives a fully validated event and a decrypted password
    /// context. Implementations are responsible for evaluating the password
    /// and returning the appropriate action (MigratePassword, UpdatePassword,
    /// Retry, or Block).
    /// </remarks>
    protected abstract Task<PasswordSubmitResponse> HandleCoreAsync(
        PasswordSubmitEvent request,
        DecryptedPasswordContext decrypted,
        CancellationToken cancellationToken);
}
