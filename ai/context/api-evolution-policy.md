# API Evolution Policy

Public APIs are long-term contracts.

Changes requiring special review:

- interface modifications
- breaking changes
- namespace moves
- package renames

Preferred changes:

- additive APIs
- optional parameters
- new extension points

Avoid:

- changing existing behavior
- removing contracts
- introducing host-specific dependencies into abstractions