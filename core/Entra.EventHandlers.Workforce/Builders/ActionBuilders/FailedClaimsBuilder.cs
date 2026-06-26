using Entra.EventHandlers.Workforce.Builders.Interfaces;
using Entra.EventHandlers.Workforce.Builders.ResponseBuilders;

namespace Entra.EventHandlers.Workforce.Builders.ActionBuilders;

public sealed class FailedClaimsBuilder(VerifiedIdClaimValidationResponseBuilder parent) : IFailedClaimsBuilder
{
    private readonly VerifiedIdClaimValidationResponseBuilder _parent = parent;
    private readonly List<string> _claims = [];

    public IFailedClaimsBuilder Add(string claimName)
    {
        _claims.Add(claimName);
        return this;
    }

    public IVerifiedIdClaimValidationResponseBuilderFinal Done()
    {
        return _parent.Failed(_claims);
    }
}
