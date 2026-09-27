# .NET Version Upgrade — Report

**Scenario:** Upgrade .NET projects to .NET 10 (net10.0)
**Outcome:** ✅ Fully completed
**Projects affected:** 1 (DatabaseSerialiser)
**Tasks:** 9/9 completed

---

## Summary

This scenario migrated the DatabaseSerialiser project from a classic .NET Framework project to a modern SDK-style project targeting .NET 10 (net10.0). Work included converting the project file, migrating package management to PackageReference, updating key NuGet packages, fixing assembly binding redirects, replacing System.Data.SqlClient usage with Microsoft.Data.SqlClient, and validating builds.

---

## What Changed

### Packages

| Project | Package | Change | From → To |
|---------|---------|--------|-----------|
| DatabaseSerialiser | Newtonsoft.Json | Updated | 13.0.1 → 13.0.4 |
| DatabaseSerialiser | Microsoft.Data.SqlClient | Added | (none) → 5.2.0 |
| DatabaseSerialiser | Microsoft.Extensions.CommandLineUtils | Kept (deprecated) | 1.1.1 → 1.1.1 |
| DatabaseSerialiser | Newtonsoft.Json.Bson | Kept | 1.0.2 → 1.0.2 |

### Code Modifications

- API migrations
  - Replaced usages of System.Data.SqlClient with Microsoft.Data.SqlClient in Database.cs (SqlConnection, SqlCommand, SqlDataReader).

- Project file changes
  - Converted DatabaseSerialiser.csproj from legacy (ToolsVersion-based) format to SDK-style (.NET SDK project). Migrated packages.config entries to PackageReference.
  - Removed temporary multi-targeting and finalized project to target net10.0.
  - Disabled SDK auto-generation of assembly attributes to preserve existing AssemblyInfo.cs semantics.

- Configuration
  - Updated App.config binding redirect for Newtonsoft.Json to newVersion 13.0.4.0 (oldVersion range 0.0.0.0-13.0.4.0) to avoid runtime version conflicts.

- Build & tooling
  - Restores and builds use `dotnet restore` / `dotnet build`. Both net472 (during migration) and net10.0 builds were validated.

### Git Commits

| SHA | Message |
|-----|---------|
| a8fc8a7 | upgrade(07-finalize-and-cleanup): finalize upgrade to .NET 10, update docs |
| bf4187a | upgrade(06-build-and-integration-validation): build and validate solution |
| 3e098de | upgrade(04-fix-binding-redirects): update Newtonsoft.Json binding redirect to 13.0.4.0 |
| 17adfb1 | upgrade(03-update-nuget-packages): update Newtonsoft.Json to 13.0.4 |
| 6b77ecf | upgrade(05-migrate-sqlclient-apis): migrate System.Data.SqlClient usages to Microsoft.Data.SqlClient |
| a4130d9 | upgrade(01.03-validate-and-commit): validate SDK conversion and commit |
| 6644b08 | upgrade(01.02-convert-project-file): convert project to SDK-style |

(Full commit history available in the repository git log on branch `upgrade-dotnet-10`.)

---

## Task Breakdown

| Task | Description | Outcome | Links |
|------|-------------|---------|-------|
| 01-convert-to-sdk | Convert DatabaseSerialiser.csproj to SDK-style | ✅ Completed | [task](tasks/01-convert-to-sdk/task.md), [progress](tasks/01-convert-to-sdk/progress-details.md) |
| 02-update-tfms-and-project-file | Update project TFMs to net10.0 | ✅ Completed | [task](tasks/02-update-tfms-and-project-file/task.md), [progress](tasks/02-update-tfms-and-project-file/progress-details.md) |
| 03-update-nuget-packages | Upgrade NuGet packages to net10-compatible versions | ✅ Completed | [task](tasks/03-update-nuget-packages/task.md), [progress](tasks/03-update-nuget-packages/progress-details.md) |
| 04-fix-binding-redirects | Fix binding redirects and assembly policies | ✅ Completed | [task](tasks/04-fix-binding-redirects/task.md), [progress](tasks/04-fix-binding-redirects/progress-details.md) |
| 05-migrate-sqlclient-apis | Replace System.Data.SqlClient usages | ✅ Completed | [task](tasks/05-migrate-sqlclient-apis/task.md), [progress](tasks/05-migrate-sqlclient-apis/progress-details.md) |
| 06-build-and-integration-validation | Build solution and validate behavior | ✅ Completed | [task](tasks/06-build-and-integration-validation/task.md), [progress](tasks/06-build-and-integration-validation/progress-details.md) |
| 07-finalize-and-cleanup | Final commits, documentation and report | ✅ Completed | [task](tasks/07-finalize-and-cleanup/task.md), [progress](tasks/07-finalize-and-cleanup/progress-details.md) |

---

## Decisions Made

- **Target Framework** — Upgraded to net10.0 (finalized). — User requested .NET 10.
- **Commit Strategy** — After Each Task. Commits were created after each major task.
- **Project file approach** — Converted to SDK-style and migrated packages to PackageReference; temporarily multi-targeted for validation, then finalized to net10.0.

---

## Build & Test Results

- Solution builds: ✅ net10.0 build succeeded (artifacts: `bin\Debug\net10.0\DatabaseSerialiser.dll`).
- Legacy build during migration: ✅ net472 build succeeded when used for validation.
- Tests: No unit or integration tests detected in the solution.
- Warnings: NuGet advisory warnings reported for `Azure.Identity` and `Microsoft.Identity.Client`; MSBuild reported some version-unification warnings for Newtonsoft.Json (resolved via binding redirect).

---

## Known Gaps & Follow-up Items

- **Replace deprecated package** — Microsoft.Extensions.CommandLineUtils remains referenced (1.1.1). Consider replacing with a maintained CLI parser or `System.CommandLine`.
- **NuGet advisories** — Packages `Azure.Identity` and `Microsoft.Identity.Client` show advisory warnings; review and update these packages if your security policy requires it.
- **Add tests** — No automated tests were present. Adding unit/integration tests would help validate behavior after upgrades.

---

Report generated: final-report.md in the scenario folder.

