# 02-update-tfms-and-project-file: Update project target framework(s) to net10.0

Update the project's TargetFramework (or TargetFrameworks) to include net10.0 and remove net472-only settings. Ensure any conditional compilation symbols and import targets compatible with .NET 10 are updated. If multi-targeting is needed for incremental migration, add net10.0 alongside net472 and validate compilation for net10.0.

Affected items: DatabaseSerialiser.csproj

Done when: Project TFM includes net10.0 and the project restores and compiles successfully for net10.0 (build may require fixes from subsequent tasks).
