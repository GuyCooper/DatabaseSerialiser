# .NET Version Upgrade Plan

## Overview

**Target**: DatabaseSerialiser solution → upgrade projects from .NET Framework 4.7.2 to net10.0
**Scope**: Single classic .NET project (DatabaseSerialiser.csproj), ~886 LOC. Work includes project conversion to SDK-style, package upgrades, binding redirect fixes, and source-incompatible API updates.

## Tasks

### 01-convert-to-sdk: Convert DatabaseSerialiser.csproj to SDK-style

Convert the existing classic (non-SDK) project file to the SDK-style project format. Replace packages.config or legacy references with PackageReference entries where applicable, remove legacy assembly binding redirects from the project, and simplify the project file to a minimal SDK-style manifest.

Affected items: DatabaseSerialiser.csproj

Done when: The project file is an SDK-style .csproj, restores packages with dotnet restore, and the project builds targeting net10.0 when temporarily multi-targeting (see task 02) or after TFM update.

---

### 02-update-tfms-and-project-file: Update project target framework(s) to net10.0

Update the project's TargetFramework (or TargetFrameworks) to include net10.0 and remove net472-only settings. Ensure any conditional compilation symbols and import targets compatible with .NET 10 are updated. If multi-targeting is needed for incremental migration, add net10.0 alongside net472 and validate compilation for net10.0.

Affected items: DatabaseSerialiser.csproj

Done when: Project TFM includes net10.0 and the project restores and compiles successfully for net10.0 (build may require fixes from subsequent tasks).

---

### 03-update-nuget-packages: Upgrade NuGet packages to net10-compatible versions

Update packages identified by the assessment (e.g., Newtonsoft.Json to a supported 13.x or newer compatible version, replace deprecated Microsoft.Extensions.CommandLineUtils with maintained alternatives). Prefer PackageReference and ensure package versions support net10.0. Re-run restore and resolve any package-reference conflicts.

Affected items: packages referenced by DatabaseSerialiser.csproj

Done when: All referenced packages resolve and restore for net10.0 and no package incompatibility issues remain.

---

### 04-fix-binding-redirects: Remove or correct binding redirects and assembly binding policies

Remove conflicting manual binding redirects from configuration and let SDK/MSBuild manage redirects, or update redirect versions to match package versions. Ensure app.config/web.config contains no redirects that force incompatible versions.

Affected items: app.config (or other config files), project properties

Done when: No binding-redirect conflicts reported by the build and runtime assembly loading is consistent.

---

### 05-migrate-sqlclient-apis: Replace System.Data.SqlClient usages with Microsoft.Data.SqlClient or compatible APIs

Update code that uses System.Data.SqlClient types where source incompatibilities were flagged. Replace with Microsoft.Data.SqlClient usage or adjust calls to match API differences. Run targeted builds and unit tests for affected code paths.

Affected items: source files using SqlClient (see assessment.md for list)

Done when: Code compiles for net10.0 and no source-incompatible API errors remain for SqlClient-related code.

---

### 06-build-and-integration-validation: Build solution, run tests, and validate behavior

Perform a clean build and run any available unit/integration tests. Validate that the application runs and serializes/deserializes data as before. Fix any runtime errors reported and re-run tests.

Done when: Solution builds without errors, all tests pass (if present), and basic manual validation of serialization behavior succeeds.

---

### 07-finalize-and-cleanup: Final commits, documentation and report

Update README/notes with upgrade steps taken, remove any temporary multi-targeting flags, and make final commits per commit strategy.

Done when: Documentation updated, temporary flags removed, all changes committed to working branch, and plan progress updated.
