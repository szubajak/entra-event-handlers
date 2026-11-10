# API Evolution Policy

Public APIs are long-term contracts.

Changes requiring special review:

- interface modifications
- breaking changes
- namespace moves
- package renames

Preferred changes:

- additive APIs
- additive extension methods
- new extension points

Avoid:

- changing existing behavior
- removing contracts
- introducing host-specific dependencies into abstractions

Additional compatibility rules:

- Do not modify public interfaces casually. Adding an interface member can break third-party implementations.
- Consider a new interface, extension method, derived contract, or additive implementation type when compatibility is at risk.
- Treat event models, response models, handler interfaces, `EntraHandlerResult<TResponse>`, and package metadata as public contracts.
- Preserve cancellation-token behavior when changing asynchronous APIs.
- Evaluate package versioning and changelog impact for externally visible changes.
