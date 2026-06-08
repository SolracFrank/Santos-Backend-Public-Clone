# AGENTS.md

## Project overview

This repository contains a .NET API (Database first) that is being reorganized from a single-project structure into a layered architecture.

The base repository is `API-main`.

The Web API project is located at:

```txt
API-main/API-main/EsApi.csproj
```

The rest of the solution is organized under the first-level `API-main/` directory into separate projects/layers.

The intended architecture is:

```txt
API-main/
├─ Domain/Domain.csproj
├─ Application/Application.csproj
├─ Infrastructure/Infrastructure.csproj
└─ API-main/esApi.csproj
```

Use the existing solution structure as the source of truth. If the folder/project layout differs from this document, inspect the repository before making assumptions.

## Architectural intent

The project should follow a layered architecture with clear boundaries.

### Web API layer (esApi.csproj)

The Web API project is responsible for HTTP concerns only.

It may contain:

- Controllers or endpoints
- API configuration
- Dependency injection composition
- Middleware registration
- Authentication/authorization wiring
- Request/response surface concerns
- Swagger/OpenAPI configuration
- Filters

It should not contain business logic, persistence logic, or domain rules that belong in `Application`, `Domain`, or `Infrastructure`.

### Domain layer

The `Domain` project contains the core domain types and shared model definitions.

It may contain:

- Entities
- Domain models
- DTOs when they are intentionally shared across layers
- Configuration/model classes
- Domain exceptions
- Value objects, if present
- Enums and constants related to the domain

Domain should stay as dependency-light as possible.

### Application layer

The `Application` project contains business logic and use-case orchestration.

The intended Application architecture is:
Application/
├─ Application/Application.csproj
└─ Features
└─ ApiEquivalent with Queries, Handlers and Validators

Application is the main place for:

- Commands and handlers
- Queries and handlers
- Business rules
- Application services
- FluentValidation validators
- Result handling
- Use-case-level logging
- Interfaces for external dependencies when the abstraction belongs to the use case

Prefer Command/Query organization for use cases.

Business logic should generally live here, not in controllers and not in EF Core configuration.

### Infrastructure layer

The `Infrastructure` project contains implementation details.

It may contain:

- `Data/AppDbContext` - DbContext
- EF Core configuration classes under `Data/Configuration`
- Repositories, if used
- External service implementations
- Persistence-related services
- Service implementations under `Services`
- Service interfaces under `Services/Interfaces`, if that is the existing project convention

## Validation conventions

Use FluentValidation for request/use-case validation.

Validators should be explicit and close to the command/query they validate when possible.

Do not duplicate the same validation logic across controllers and handlers.

Controller-level validation should be limited to HTTP-specific concerns. Business validation belongs in Application or Domain depending on the rule.

## Result handling

The project uses LanguageExt-style result handling only for Command-Handlers responses.

Prefer returning explicit result types from Application use cases instead of throwing exceptions for expected business failures.

Follow the existing project conventions when handling `Result`.

Do not replace LanguageExt result flow with ad-hoc booleans, null returns, or broad exception swallowing.

Expected failures should be modeled and returned.

Unexpected failures may be logged and propagated according to the current error-handling pattern.

## Logging conventions

Use dependency-injected logging with `ILogger<T>`.

Prefer structured logging:

```csharp
_logger.LogInformation("Creating customer with id {CustomerId}", customerId);
_logger.LogWarning("Customer {CustomerId} was not found", customerId);
_logger.LogError(ex, "Failed to create customer {CustomerId}", customerId);
```

Do not use string interpolation inside logging templates when structured logging is appropriate.

Avoid logging sensitive information such as passwords, tokens, full credentials, or private personal data.

Log at the appropriate level:

- `LogDebug` for diagnostic development details
- `LogInformation` for normal business/application milestones
- `LogWarning` for recoverable or suspicious conditions
- `LogError` for failures requiring attention

## Services conventions

Infrastructure service implementations may live under:

```txt
Infrastructure/Services/
```

Interfaces may live under:

```txt
Infrastructure/Services/Interfaces/
```

Follow the existing project convention unless there is a clear architectural reason to move an interface into Application.

## Error handling

As for now API is using ProblemDetails conventions for error handling (returning errors) depending on the result.

## Controllers and endpoints

Controllers/endpoints should be thin.

They should:

- use Swagger properties for documentation (Description, expected result (including possible errors), etc.)
- Receive HTTP input
- Call Application commands/queries
- Map Application results to HTTP responses
- Avoid direct logic

Do not place persistence or business rules inside controllers.

## CodeGraph usage

This project may use CodeGraph code-navigation tooling.

When available, use CodeGraph to inspect relationships before making structural changes, especially for:

- Moving classes between projects
- Renaming public types
- Refactoring handlers/services
- Changing dependency direction
- Removing apparently unused code
- Modifying shared DTOs/models
- Updating interfaces and implementations

Do not assume a type is unused based only on local inspection. Check references first.

## Refactoring rules

Refactoring must preserve behavior unless the task explicitly requests a behavior change.

Prefer small, mechanical, reviewable changes.

Good refactors:

- Extracting methods from large handlers/controllers
- Moving business logic from controllers to Application
- Moving EF mapping into configuration classes
- Splitting large services by responsibility
- Removing duplicated validation by centralizing it in validators
- Improving naming when it does not create excessive churn
- Replacing unclear control flow with clearer equivalent logic

Risky refactors requiring extra care:

- Changing public contracts
- Renaming DTO properties
- Changing serialization behavior
- Changing EF relationships
- Changing cascade delete behavior
- Changing tracking behavior
- Changing transaction boundaries
- Changing exception/result semantics
- Moving classes across projects
- Adding new dependencies
- Introducing new frameworks/patterns

Do not perform broad rewrites unless explicitly requested.

## Build and verification

After code changes, run the narrowest relevant verification available.

Prefer:

```bash
dotnet build
```

or, when appropriate:

```bash
dotnet build API-main/API-main/EsApi.csproj
```

If tests exist, run the relevant test project.

If a command cannot be run in the current environment, state that clearly and explain what should be run locally.

Do not claim that something builds if it was not actually built.

## Git workflow

Before editing, inspect the current branch and working tree when possible:

```bash
git branch --show-current
git status --short
```

Do not discard user changes.

Do not overwrite uncommitted work.

If the user asks the agent to work in an isolated branch, create a branch using the current branch name plus `-agent`.

Example:

```txt
current branch: feature/api-cleanup
agent branch: feature/api-cleanup-agent
```

Do not commit unless the user explicitly asks for commits or the current workflow requires it.

Use the skill 'github-automatitation' when possible.

## Naming conventions

Use clear, domain-specific names.
