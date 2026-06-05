# Entra.EventHandlers.AzureFunctions

**License:** Business Source License (BSL)  
**Author:** Jakub Szubarga (Szubarga.NET)

This package provides the **Azure Functions hosting adapter** for the Entra Event Handlers ecosystem. It enables production‑ready **Microsoft Entra External ID Authentication Event Handler** extensions to run inside Azure Functions with minimal boilerplate, full DI support, structured error handling, and complete testability.

---

## ✨ What This Package Provides

### ✔ Full routing pipeline (recommended)

The core of this package is the `EntraEventRouterFunctionBase`, which provides:

- Automatic request deserialization  
- Automatic handler resolution  
- Automatic handler invocation  
- Automatic response serialization  
- Structured error mapping  
- Logging for expected and unexpected exceptions  

This enables a **single Azure Function** to host **multiple Entra event types** cleanly.

```csharp
public abstract class EntraEventRouterFunctionBase(
    ILogger<EntraEventRouterFunctionBase> logger,
    IEntraEventHandlerResolver resolver,
    IHttpRequestAdapter requestAdapter,
    IHttpResponseAdapter responseAdapter)
{
    private readonly ILogger<EntraEventRouterFunctionBase> _logger = logger;
    private readonly IEntraEventHandlerResolver _resolver = resolver;
    private readonly IHttpRequestAdapter _requestAdapter = requestAdapter;
    private readonly IHttpResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        try
        {
            var evt = await _requestAdapter.ReadEvent(req);
            var handler = _resolver.Resolve(evt.GetType());

            var response = await ((dynamic)handler).Handle((dynamic)evt, context.CancellationToken);
            return await _responseAdapter.From(req, response);
        }
        catch (Exception ex) when (ex is EntraValidationException or EntraDeserializationException or EntraHandlerNotFoundException)
        {
            _logger.LogWarning(ex, "Handled expected Entra exception.");

            var code = ex switch
            {
                EntraValidationException => EntraErrorCodes.ValidationError,
                EntraDeserializationException => EntraErrorCodes.DeserializationError,
                EntraHandlerNotFoundException => EntraErrorCodes.HandlerNotFound,
                _ => throw new InvalidOperationException("Unreachable: catch filter guarantees only known Entra exceptions.")
            };

            return await _responseAdapter.BadRequest(
                req,
                new EntraErrorResponse
                {
                    Error = code,
                    Details = ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing Entra event.");

            return await _responseAdapter.ServerError(
                req,
                new EntraErrorResponse
                {
                    Error = EntraErrorCodes.UnhandledException,
                    Details = "An unexpected error occurred."
                });
        }
    }
}
```

### ✔ Minimal derived Azure Function

```csharp
public sealed class EntraRouterFunction : EntraEventRouterFunctionBase
{
    public EntraRouterFunction(
        ILogger<EntraEventRouterFunctionBase> logger,
        IEntraEventHandlerResolver resolver,
        IHttpRequestAdapter requestAdapter,
        IHttpResponseAdapter responseAdapter)
        : base(logger, resolver, requestAdapter, responseAdapter) {}

    [Function("EntraRouter")]
    public Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)
        => Run(req, ctx);
}
```

---

## 🧩 Request & Response Adapters

### IHttpRequestAdapter

```csharp
public interface IHttpRequestAdapter
{
    Task<TEvent> ReadEvent<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent;

    Task<EntraEvent> ReadEvent(HttpRequestData req);
}
```

### HttpRequestAdapter

```csharp
public sealed class HttpRequestAdapter : IHttpRequestAdapter
{
    public async Task<TEvent> ReadEvent<TEvent>(HttpRequestData req)
        where TEvent : EntraEvent =>
        await JsonSerializer.DeserializeAsync<TEvent>(req.Body)
            ?? throw new EntraDeserializationException("Unable to deserialize event.");

    public Task<EntraEvent> ReadEvent(HttpRequestData req) =>
        ReadEvent<EntraEvent>(req);
}
```

### IHttpResponseAdapter

```csharp
public interface IHttpResponseAdapter
{
    Task<HttpResponseData> From(HttpRequestData req, EntraEventResponse response);
    Task<HttpResponseData> BadRequest(HttpRequestData req, EntraErrorResponse error);
    Task<HttpResponseData> ServerError(HttpRequestData req, EntraErrorResponse error);
}
```

### HttpResponseAdapter

```csharp
public sealed class HttpResponseAdapter : IHttpResponseAdapter
{
    public async Task<HttpResponseData> From(HttpRequestData req, EntraEventResponse response)
    {
        var http = req.CreateResponse(HttpStatusCode.OK);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, response);
        return http;
    }

    public Task<HttpResponseData> BadRequest(HttpRequestData req, EntraErrorResponse error) =>
        WriteError(req, HttpStatusCode.BadRequest, error);

    public Task<HttpResponseData> ServerError(HttpRequestData req, EntraErrorResponse error) =>
        WriteError(req, HttpStatusCode.InternalServerError, error);

    private static async Task<HttpResponseData> WriteError(HttpRequestData req, HttpStatusCode status, EntraErrorResponse error)
    {
        var http = req.CreateResponse(status);
        http.Headers.Add("Content-Type", "application/json");
        await JsonSerializer.SerializeAsync(http.Body, error);
        return http;
    }
}
```

---

## 🧠 Handler Resolution

Handlers are resolved dynamically based on the event type:

```csharp
public interface IEntraEventHandlerResolver
{
    IEntraEventHandler Resolve(Type eventType);
}
```

This allows multiple event types to be hosted behind a single Azure Function.

---

## 🛠 Dependency Injection

Register everything with a single extension:

```csharp
public static class EntraEventHandlersFunctionExtensions
{
    public static IServiceCollection AddEntraEventHandlersForFunctions(this IServiceCollection services)
    {
        services.AddSingleton<IHttpRequestAdapter, HttpRequestAdapter>();
        services.AddSingleton<IHttpResponseAdapter, HttpResponseAdapter>();
        services.AddSingleton<IEntraEventHandlerResolver, EntraEventHandlerResolver>();

        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IEntraEventHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
```

---

## 🧪 Unit Testing

The router is fully testable thanks to the abstractions.

Here is a real example of a unit test verifying deserialization failure handling:


```csharp
[Fact]
public async Task Run_WhenDeserializationFails_ReturnsBadRequest()
{
    // Arrange
    var ctx = Substitute.For<FunctionContext>();
    var request = Substitute.For<HttpRequestData>(ctx);
    var response = Substitute.For<HttpResponseData>(ctx);

    var exception = new EntraDeserializationException("bad");
    _requestAdapter.ReadEvent(request).Throws(exception);

    _responseAdapter
        .BadRequest(request, Arg.Any<EntraErrorResponse>())
        .Returns(response);

    // Act
    var result = await _sut.RunAsync(request, ctx);

    // Assert
    _responseAdapter.Received(1).BadRequest(
        request,
        Arg.Is<EntraErrorResponse>(e =>
            e.Error == EntraErrorCodes.DeserializationError &&
            e.Details == "bad"));

    _logger.Entries.Should().ContainSingle(e =>
        e.Level == LogLevel.Warning &&
        e.Exception == exception);
}
```

No real HTTP streams, no real Azure Functions runtime, no static calls.

---

## 📦 Base Classes (Optional)

You can use the simple single‑event hosting model:

```csharp
public abstract class TokenIssuanceStartFunctionBase(
    ITokenIssuanceStartHandler handler,
    IHttpRequestAdapter requestAdapter,
    IHttpResponseAdapter responseAdapter)
{
    private readonly ITokenIssuanceStartHandler _handler = handler;
    private readonly IHttpRequestAdapter _requestAdapter = requestAdapter;
    private readonly IHttpResponseAdapter _responseAdapter = responseAdapter;

    protected async Task<HttpResponseData> Run(HttpRequestData req, FunctionContext context)
    {
        var evt = await _requestAdapter.ReadEvent<TokenIssuanceStartEvent>(req);
        var response = await _handler.Handle(evt, context.CancellationToken);
        return await _responseAdapter.From(req, response);
    }
}
```

---

## 🔒 License

This package is licensed under the **Business Source License (BSL)**.

See:

- **LICENSE** — full BSL terms  
- **LICENSE-COMMERCIAL.md** — commercial licensing terms  

A commercial license is required for production use by organizations with more than 5 employees.

### Commercial License Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

To purchase a license or request an invoice:

📧 **jakub.szubarga@gmail.com**

The abstractions package is MIT‑licensed and can be used freely.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the main repository as the ecosystem evolves.
