using Entra.EventHandlers.Abstractions.Interfaces;
using Entra.EventHandlers.AspNetCore.Adapters;
using Microsoft.AspNetCore.Http;

namespace Entra.EventHandlers.AspNetCore.Base;

public abstract class AttributeCollectionStartEndpointBase(
    IAttributeCollectionStartHandler handler,
    IAspNetCoreRequestAdapter requestAdapter,
    IAspNetCoreResponseAdapter responseAdapter)
{
    private readonly IAttributeCollectionStartHandler _handler = handler;
    private readonly IAspNetCoreRequestAdapter _requestAdapter = requestAdapter;
    private readonly IAspNetCoreResponseAdapter _responseAdapter = responseAdapter;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        //// Read event from ASP.NET Core HttpContext
        //var evt = await _requestAdapter.ReadEvent<AttributeCollectionStartEvent>(httpContext);

        //// Execute handler
        //var response = await _handler.Handle(evt, httpContext.RequestAborted);

        //// Write response to HttpContext
        //await _responseAdapter.WriteAsync(httpContext, response);
    }
}

