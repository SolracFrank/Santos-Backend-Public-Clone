---
name: codegraph
description: Use CodeGraph MCP for safe refactoring and code navigation.
---

# CodeGraph Skill

Use CodeGraph before non-trivial refactors.

Workflow:
1. Find target symbol.
2. Find callers.
3. Find callees.
4. Identify affected files.
5. Read source files.
6. Create a refactor plan.
7. Apply smallest safe change.
8. Re-check references.
9. Build and verify.

Never trust graph results without reading source code.
