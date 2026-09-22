using System.Security.Cryptography;

namespace Entra.EventHandlers.Security.Providers;

public interface ICertificateProvider
{
    Task<RSA> GetRsaAsync(CancellationToken cancellationToken = default);
}
