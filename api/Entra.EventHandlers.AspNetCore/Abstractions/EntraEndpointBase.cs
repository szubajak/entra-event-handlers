using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Entra.EventHandlers.AspNetCore.Abstractions;

public abstract class EntraEndpointBase(IRequestAdapter requestAdapter, IResponseAdapter responseAdapter)
{
    protected IRequestAdapter RequestAdapter { get; } = requestAdapter;
    protected IResponseAdapter ResponseAdapter { get; } = responseAdapter;

    protected abstract Task Invoke(HttpContext httpContext);

    public abstract void Map(IEndpointRouteBuilder endpoints);
}
