# Entra.EventHandlers.Abstractions

**License:** MIT  
**Author:** Jakub Szubarga — Szubarga.NET

This package contains the public abstractions, event types, and protocol
definitions for building Entra ID Authentication Event Handlers.

It is intentionally lightweight, dependency‑free, and framework‑agnostic.
You can reference it freely in open‑source or commercial projects.

All public types include full XML documentation for a first‑class developer
experience.

---

## ✨ What This Package Provides

This package defines the **public contract** for the Entra Event Handlers
ecosystem:

- Strongly‑typed event request models  
- Response models and action definitions  
- Event type constants  
- Protocol primitives  
- Interfaces for building custom handlers  
- Enums and metadata types  

These types represent the JSON protocol used by Entra ID custom authentication
flows.

The abstractions are stable, versioned, and safe to depend on in long‑term
projects.

---

## 🧩 Why a Separate Abstractions Package?

The abstractions are MIT‑licensed to maximize adoption and interoperability.

They allow you to:

- Build your own handlers  
- Integrate with Entra ID events  
- Test locally  
- Reference the protocol without pulling in implementation details  

The full implementation lives in separate packages under the Business Source
License (BSL).

---

## 📦 Related Packages

These packages extend the abstractions with production‑ready functionality:

- **Entra.EventHandlers** — full implementation (BSL)  
  - Validation  
  - Routing  
  - Execution pipeline  
  - Response builders  
  - Logging & telemetry hooks  

- **Entra.EventHandlers.AzureFunctions** — Azure Function integration (BSL)  
  - Automatic request/response handling  
  - DI wiring  
  - Minimal boilerplate for production deployments  

These packages will be published to NuGet soon.

---

## 📄 License

This package is licensed under the MIT License.  
See the [LICENSE](./LICENSE) file for details.

The full implementation and Azure Function integration are available under the
Business Source License (BSL) in the related packages.

---

## 📚 Documentation

Full documentation, examples, and production templates will be available in the
main repository.
