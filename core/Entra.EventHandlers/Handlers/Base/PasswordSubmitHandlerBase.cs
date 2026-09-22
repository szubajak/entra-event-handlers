using Entra.EventHandlers.Abstractions.Events;
using Entra.EventHandlers.Abstractions.Extensions;
using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.Abstractions.Protocol.PasswordSubmit;
using Entra.EventHandlers.Abstractions.Responses;
using Entra.EventHandlers.Abstractions.Results;
using Entra.EventHandlers.Builders;
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
public abstract class PasswordSubmitHandlerBase(ILogger logger, IPasswordContextDecryptor decryptor) : IPasswordSubmitHandler
{
    protected ILogger Logger { get; } = logger;
    protected IPasswordContextDecryptor Decryptor { get; } = decryptor;

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
    public async Task<EntraHandlerResult<PasswordSubmitResponse>> HandleAsync(PasswordSubmitEvent request, CancellationToken cancellationToken = default)
    {
        using var scope = Logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = request.CorrelationId,
            ["EventType"] = request.Type,
            ["EventName"] = request.GetType().Name
        });

        var sw = Stopwatch.StartNew();

        Logger.LogInformation("Starting Entra event handling.");

        DecryptedPasswordContext? decrypted = null;

        try
        {
            request.Validate();

            decrypted = Decryptor.Decrypt(request.Data.EncryptedPasswordContext);

            var response = await HandleCoreAsync(request, decrypted, cancellationToken);

            sw.Stop();

            var actionType = response.Data.Actions.FirstOrDefault()?.OdataType ?? "None";

            Logger.LogInformation(
                "Entra event handled successfully. DurationMs={DurationMs}, Action={ActionType}.",
                sw.ElapsedMilliseconds,
                actionType);

            return new EntraHandlerResult<PasswordSubmitResponse>(response);
        }
        catch (Exception ex)
        {
            sw.Stop();

            if (ex.IsEntraException())
            {
                Logger.LogWarning(
                    ex,
                    "Entra domain exception occurred during Entra event handling. DurationMs={DurationMs}.",
                    sw.ElapsedMilliseconds);
            }
            else
            {
                Logger.LogError(
                    ex,
                    "Unexpected failure occurred during Entra event handling. DurationMs={DurationMs}.",
                    sw.ElapsedMilliseconds);
            }

            if (decrypted?.Nonce is string nonce)
            {
                var defaultResponse = EntraEventResponses.PasswordSubmit()
                    .WithNonce(nonce)
                    .Block()
                    .Build();

                return new EntraHandlerResult<PasswordSubmitResponse>(defaultResponse, ex);
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
        CancellationToken cancellationToken = default);
}
