[![Sponsor](https://img.shields.io/badge/Sponsor-%E2%9D%A4-red)](https://github.com/sponsors/szubajak)

# Entra Event Handlers — .NET Ecosystem

A modern, strongly‑typed, developer‑friendly ecosystem for building  
**Microsoft Entra External ID Authentication Event Handlers** in .NET.

This repository provides:

- MIT‑licensed **public abstractions**
- BSL‑licensed **core implementation**
- BSL‑licensed **Azure Functions hosting adapter**
- BSL‑licensed **ASP.NET Core hosting adapter**
- Fluent response builders
- Protocol‑accurate request/response models
- Base handler infrastructure (logging, validation, timing, correlation)
- A clean, extensible architecture designed for production workloads

---

## Packages

### **Abstractions**
[![NuGet Abstractions](https://img.shields.io/nuget/v/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)
[![License: MIT (Abstractions)](https://img.shields.io/badge/License-MIT-blue.svg)](abstractions/Entra.EventHandlers.Abstractions/LICENSE)

### **Core**
[![NuGet Core](https://img.shields.io/nuget/v/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)
[![License: BSL (Core)](https://img.shields.io/badge/License-BSL-orange.svg)](core/Entra.EventHandlers/LICENSE)

### **Azure Functions**
[![NuGet Core](https://img.shields.io/nuget/v/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)
[![License: BSL (AzureFunctions)](https://img.shields.io/badge/License-BSL-orange.svg)](functions/Entra.EventHandlers.AzureFunctions/LICENSE)

### **ASP.NET Core**
[![NuGet AspNetCore](https://img.shields.io/nuget/v/Entra.EventHandlers.AspNetCore.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AspNetCore)
[![License: BSL (AspNetCore)](https://img.shields.io/badge/License-BSL-orange.svg)](api/Entra.EventHandlers.AspNetCore/LICENSE)

---

## 🧩 Architecture Overview

The ecosystem is intentionally split into layers:

```
┌──────────────────────────────────────────────────────────┐
│ Entra.EventHandlers.Abstractions (MIT)                   │
│ Public contract, protocol types, interfaces              │
└──────────────────────────────────────────────────────────┘
                          ▲
                          │
┌──────────────────────────────────────────────────────────┐
│ Entra.EventHandlers (BSL)                                │
│ Implementation, builders, base handlers, validation      │
└──────────────────────────────────────────────────────────┘
                          ▲
                          │
┌──────────────────────────────────────────────────────────┐
│ Hosting Adapters (BSL)                                   │
│                                                          │
│ • Entra.EventHandlers.AzureFunctions                     │
│   Azure Functions hosting adapter                        │
│                                                          │
│ • Entra.EventHandlers.AspNetCore                         │
│   ASP.NET Core hosting adapter                           │
└──────────────────────────────────────────────────────────┘
```

This separation ensures:

- **Maximum adoption** (MIT abstractions)  
- **Commercial protection** (BSL implementation)  
- **Clean extensibility**  
- **Stable public API surface**  

---

## 📦 Projects in This Repository

### **1. Entra.EventHandlers.Abstractions** (MIT)

Lightweight, dependency‑free abstractions defining the public contract:

- Event request models  
- Response models and action types  
- Event and OData constants  
- Directory attribute primitives  
- Interfaces for building custom handlers  

Safe to reference in any open‑source or commercial project.

➡️ *See the package README for details.*

---

### **2. Entra.EventHandlers** (BSL)

The full implementation layer built on top of the abstractions:

- Fluent response builders  
- Strongly‑typed construction of Entra responses  
- `PrefillValuesBuilder` for attribute prefill scenarios  
- Unified entry point (`EntraEventResponses.*`)  
- Base handler infrastructure:
  - Structured logging  
  - Correlation scoping  
  - Execution timing  
  - Protocol‑level validation (`@odata.type`)  
  - Consistent exception handling  
  - Clean override point (`HandleCore`)  

➡️ *See the package README for details.*

---

### **3. Entra.EventHandlers.AzureFunctions** (BSL)

Azure Functions hosting adapter:

- Automatic request deserialization  
- Automatic handler resolution  
- Automatic response serialization  
- DI wiring  
- Minimal boilerplate  
- Router function model (multi‑event)  
- Single‑event function base classes

➡️ *See the package README for details.*

---

### **4. Entra.EventHandlers.AspNetCore** (BSL)

ASP.NET Core hosting adapter:

- Minimal API endpoint integration  
- Router endpoint model (multi‑event)  
- Single‑event endpoint base classes  
- Automatic request/response handling  
- DI integration  
- Clean, testable hosting model  

➡️ *See the package README for details.*

---

## 🛠 Example: Building a Response

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .ShowBlockPage("Error", "Unexpected error occurred.")
    .Build();
```

With prefill:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@example.com")
        .Add("country", "PL")
    .Done()
    .Build();
```

---

## 🛠 Example: Implementing a Handler

```csharp
public class TokenIssuanceStartHandler(ILogger<TokenIssuanceStartHandler> logger)
    : TokenIssuanceStartHandlerBase(logger)
{
    protected override Task<TokenIssuanceStartResponse> HandleCore(
        TokenIssuanceStartEvent request,
        CancellationToken cancellationToken)
    {
        // Extract user ID (GUID)
        var userId = request.Data.AuthenticationContext?.User?.Id;

        // Example: determine roles based on user ID
        var roles = userId switch
        {
            // Example: special admin GUID
            var id when id == Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee")
                => ["Admin", "PowerUser"],

            // Default
            _ => new[] { "User" }
        };

        // Example: add custom claims
        var customClaims = new Dictionary<string, object>
        {
            { "tenantId", "contoso-eu" },
            { "department", "Engineering" },
            { "roles", roles }
        };

        return Task.FromResult(
            EntraEventResponses
                .TokenIssuanceStart()
                .ProvideClaimsForToken(customClaims)
                .Build());
    }
}
```

The base class automatically provides:

- CorrelationId logging  
- EventType/EventName scoping  
- Duration measurement  
- Validation  
- Exception handling  

---

## 🔒 Licensing Model

This repository uses a hybrid licensing approach:

- **MIT** for the abstractions  
- **BSL** for the implementation and hosting layers  

The BSL packages:

- allow free non‑production use  
- allow limited production use (small teams)  
- automatically convert to MIT after the Change Date  

This model keeps the ecosystem open while supporting sustainable development.

---

## 💼 Commercial Licensing

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

## Pricing

- **Developer License** — €99 / developer / year  
- **Team License** — €399 / year  
- **Enterprise License** — €1499 / year  

For commercial licensing or support:

📧 **jakub.szubarga@gmail.com**

➡️ See [LICENSE-COMMERCIAL.md](LICENSE-COMMERCIAL.md) for full commercial terms.

---

## 🚀 Roadmap

Planned enhancements include:
- Handler composition (pre/post processing)
- Execution pipeline components
- Telemetry and OpenTelemetry hooks
- Test utilities and mocks
- Full documentation site
- Production templates for Azure Functions and ASP.NET Core
- Sample implementations and scenarios

---

## 📚 Documentation

Documentation, examples, and production templates will be published in the main
repository as the ecosystem evolves.

---

## 🤝 Contributing

Contributions to the MIT abstractions package are welcome.  
Implementation packages follow a controlled contribution model due to BSL.

---

## ❤️ Support the project

If you find this library useful, consider sponsoring development:

👉 https://github.com/sponsors/szubajak

---

## 🧑‍💻 Author

**Jakub Szubarga (Szubarga.NET)**


If you find this ecosystem useful, consider starring the repository ⭐
