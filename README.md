# Entra Event Handlers — .NET Ecosystem

[![Coverage](https://szubajak.github.io/entra-event-handlers/badge_shieldsio_branchcoverage_blue.svg)](https://szubajak.github.io/entra-event-handlers/)

A modern, strongly‑typed, developer‑focused ecosystem for building 
**Microsoft Entra External ID and Workforce Authentication Event Handlers** in .NET.

This repository contains:

- **MIT‑licensed abstractions** — public protocol types and contracts
- **BSL‑licensed implementation layers** — External ID + Workforce
- **BSL‑licensed hosting adapters** for Azure Functions and ASP.NET Core
- **Protocol‑accurate request/response models**
- **Production‑ready handler infrastructure** (logging, validation, timing, correlation)
- **Clean, extensible architecture** designed for real‑world workloads

---

## 📦 Packages

### **Abstractions (MIT)**  
Public protocol types and contracts.

[![NuGet Abstractions](https://img.shields.io/nuget/v/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.Abstractions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Abstractions)
[![License: MIT (Abstractions)](https://img.shields.io/badge/License-MIT-blue.svg)](abstractions/Entra.EventHandlers.Abstractions/LICENSE)

---

### **Core (BSL)**  
Implementation layer for **External ID**: builders, handler bases, validation.

[![NuGet Core](https://img.shields.io/nuget/v/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.svg)](https://www.nuget.org/packages/Entra.EventHandlers)
[![License: BSL (Core)](https://img.shields.io/badge/License-BSL-orange.svg)](core/Entra.EventHandlers/LICENSE)

---

### **Workforce (BSL)**  
Workforce‑specific event models, builders, and handler bases.

[![NuGet Workforce](https://img.shields.io/nuget/v/Entra.EventHandlers.Workforce.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Workforce)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.Workforce.svg)](https://www.nuget.org/packages/Entra.EventHandlers.Workforce)
[![License: BSL (Workforce)](https://img.shields.io/badge/License-BSL-orange.svg)](workforce/Entra.EventHandlers.Workforce/LICENSE)

---

### **Azure Functions (BSL)**  
Azure Functions hosting adapter.

[![NuGet AzureFunctions](https://img.shields.io/nuget/v/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.AzureFunctions.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AzureFunctions)
[![License: BSL (AzureFunctions)](https://img.shields.io/badge/License-BSL-orange.svg)](azurefunctions/Entra.EventHandlers.AzureFunctions/LICENSE)

---

### **ASP.NET Core (BSL)**  
ASP.NET Core hosting adapter.

[![NuGet AspNetCore](https://img.shields.io/nuget/v/Entra.EventHandlers.AspNetCore.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AspNetCore)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Entra.EventHandlers.AspNetCore.svg)](https://www.nuget.org/packages/Entra.EventHandlers.AspNetCore)
[![License: BSL (AspNetCore)](https://img.shields.io/badge/License-BSL-orange.svg)](aspnetcore/Entra.EventHandlers.AspNetCore/LICENSE)

---

## 🧩 Architecture Overview

```
                      ┌────────────────────────────────────────┐
                      │ Entra.EventHandlers.Abstractions       │
                      │                                        │
                      │ • Public contract                      │
                      │ • Protocol types                       │
                      │ • Interfaces                           │
                      │ • Error model                          │
                      └────────────────────────────────────────┘
                             ▲            ▲             ▲
                             │            │             │
                             │            │             │
                             │            │             │
┌────────────────────────────┴─────────┐  │  ┌──────────┴───────────────────────────┐
│ Entra.EventHandlers (External ID)    │  │  │ Entra.EventHandlers.Workforce        │
│                                      │  │  │                                      │
│ • Strongly‑typed models              │  │  │ • Workforce models                   │
│ • Builders, base handlers            │  │  │ • Builders, base handlers            │
│ • Validation, logging, timing        │  │  │ • Validation, logging, timing        │
└───────┬──────────────────────────────┘  │  └───────────────────────────────┬──────┘
        ▼                                 │                                  ▼
        │                                 │                                  │
        │          ┌──────────────────────┴───────────────────────┐          │
        │          │                                              │          │
        │          │  ┌────────────────────────────────────────┐  │          │
        │          │  │ Entra.EventHandlers.Hosting (internal) │  │          │
        │          │  │                                        │  │          │
        │          │  │ • Orchestration pipeline               │  │          │
        │          │  │ • Handler resolution                   │  │          │
        │          │  │ • DI helpers                           │  │          │
        │          │  │ • Exception mapping                    │  │          │
        │          │  └────────────────────────────────────────┘  │          │
        │          ▲                      ▲                       ▲          │
        │          │                      │                       │          │
        │          │                      │                       │          │
        └─────┬────┴──────────────────────┴───────────────────────┴────┬─────┘
              ▲                                                        ▲ 
              ▼                                                        ▼ 
┌─────────────┴────────────────────────┐     ┌─────────────────────────┴────────────┐
│ Entra.EventHandlers.AzureFunctions   │     │ Entra.EventHandlers.AspNetCore       │
│                                      │     │                                      │
│ • Router function (multi‑event)      │     │ • Router endpoint (multi‑event)      │
│ • Function bases                     │     │ • Endpoint base classes              │
│ • DI integration                     │     │ • DI integration                     │
│ • Request/response adapters          │     │ • Request/response adapters          │
└──────────────────────────────────────┘     └──────────────────────────────────────┘

```

This layered design provides:

- **Maximum adoption** — MIT‑licensed public abstractions
- **Commercial protection** — BSL‑licensed implementation and hosting layers
- **Clear separation of concerns**
- **Stable, dependency‑free public API surface**

---

## 📦 Projects in This Repository

### **1. Entra.EventHandlers.Abstractions** (MIT)

Lightweight, dependency‑free abstractions defining the public contract:

- Event request/response models
- Action definitions and protocol constants
- Directory attribute primitives
- Handler interfaces

➡️ *See the package README for details.*

---

### **2. Entra.EventHandlers** (BSL)

Implementation layer for **External ID authentication events**:

- Strongly‑typed request/response models
- Fluent response builders
- Base handler classes
- Validation, logging, correlation, execution timing
- Unified entry point: `EntraEventResponses.*`

➡️ *See the package README for details.*

---

### **3. Entra.EventHandlers.Workforce** (BSL)

Implementation layer for **Workforce authentication events**:

- Strongly‑typed request/response models (e.g., VerifiedIdClaimValidation)
- Fluent Workforce response builders
- Workforce handler base classes
- Validation, logging, correlation, execution timing
- Unified entry point: `EntraWorkforceEventResponses.*`

➡️ *See the package README for details.*

---

### **4. Entra.EventHandlers.AzureFunctions** (BSL)

Azure Functions hosting adapter for Entra Event Handlers:

- Multi‑event router function
- Single‑event function base classes
- DI integration
- Structured error handling
- Request/response adapters
- Built on the shared hosting layer (orchestration, handler resolution)

➡️ *See the package README for details.*

---

### **5. Entra.EventHandlers.AspNetCore** (BSL)

ASP.NET Core hosting adapter for Entra Event Handlers:

- Minimal API endpoint integration
- Multi‑event router endpoint
- Single‑event endpoint classes
- Clean, testable hosting model
- Request/response adapters
- Built on the shared hosting layer (orchestration, handler resolution)

➡️ *See the package README for details.*

---

## 📁 Samples

This repository includes complete, production‑ready samples for all hosting models.

### **ASP.NET Core**
- **ApiSample** — minimal API application using router + single‑event endpoints.

### **Azure Functions**
- **AzureFunctionsSample** — minimal Function App using router + single‑event functions.

### **Shared Handler Logic**
- **Sample.Common** — shared sample handlers used by both hosting models.

You can find the samples under the [samples](./samples) directory.

---

## 🛠 Example: Building a Response

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .ShowBlockPage("Error", "Unexpected error occurred.")
    .Build();
```

With attribute prefill:

```csharp
return EntraEventResponses
    .AttributeCollectionStart()
    .SetPrefillValues()
        .Add("email", "user@example.com")
    .Done()
    .Build();
```

For more examples, see the package READMEs and the samples in the [samples](./samples) directory.

---

## 🛠 Example: Implementing a Handler

```csharp
public class AttributeCollectionStartHandler(ILogger<AttributeCollectionStartHandler> logger)
    : AttributeCollectionStartHandlerBase(logger)
{
    protected override Task<AttributeCollectionStartResponse> HandleCore(
        AttributeCollectionStartEvent request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            EntraEventResponses
                .AttributeCollectionStart()
                .ContinueWithDefaultBehavior()
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

This repository uses a hybrid licensing model:

- **MIT** for the abstractions
- **BSL** for the implementation and hosting layers

The BSL packages:

- allow free non‑production use
- allow limited production use (small teams)
- automatically convert to MIT after the Change Date

---

## 💼 Commercial Licensing

A commercial license covers the entire **Entra Event Handlers** ecosystem, including all current and future BSL‑licensed packages.

### Pricing

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
- Telemetry and OpenTelemetry integration
- Test utilities and mocks
- Full documentation site

---

## 📚 Documentation

Documentation and additional guides will be expanded as the ecosystem evolves. 
For now, see the package READMEs and the samples in the [samples](./samples) directory.

---

## 📘 Further Reading

For a deeper look into Microsoft Entra External ID and Workforce Authentication Event Handlers, see:

➡️ **Entra External ID — .NET Handlers Deep Dive**
https://medium.com/@jakub.szubarga/entra-external-id-dotnet-handlers-a7447dc1e437

---

## 🤝 Contributing

Contributions to the MIT abstractions package are welcome. 
The implementation packages follow a controlled contribution model due to BSL.

---

## ❤️ Support the project

If you find this library useful, consider sponsoring development:

[![Sponsor](https://img.shields.io/badge/Sponsor-%E2%9D%A4-red)](https://github.com/sponsors/szubajak)

👉 https://github.com/sponsors/szubajak

---

## 🧑‍💻 Author

**Jakub Szubarga (Szubarga.NET)**

If you find this ecosystem useful, consider starring the repository ⭐
