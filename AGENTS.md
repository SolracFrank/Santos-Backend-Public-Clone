# Repository Guide

## Project layout

- `API-main/EsApi.csproj` owns HTTP routing, authentication, dependency injection, OpenAPI, and Problem Details.
- `Application/` owns commands, queries, business rules, and FluentValidation validators. Keep each validator with its feature.
- `Domain/` owns entities, shared models, and domain errors. Keep dependencies minimal.
- `Infrastructure/` owns the database-first EF Core model, mappings, repositories, and external services. Follow the existing interface locations.

## Changes

- Keep controllers thin. Map application results to HTTP responses and document expected responses in OpenAPI.
- Use explicit LanguageExt results for command outcomes. Follow the existing query contracts. Log and propagate unexpected errors through the existing error path.
- Use structured `ILogger<T>` messages. Keep credentials, tokens, and personal data out of logs.
- Preserve behavior during refactors. Check callers before changing contracts, serialization, EF relationships, or transaction boundaries. Use CodeGraph if available.
- Check the branch and working tree before editing. Preserve existing work. Use `<branch>-agent` when asked for an isolated branch. Commit only when requested.

## Verification

```sh
dotnet build API-main/EsApi.csproj
dotnet test Tests/EsApi.Tests/EsApi.Tests.csproj
```

Run the relevant checks and report any that could not run.
