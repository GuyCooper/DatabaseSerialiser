# 03-update-nuget-packages: Upgrade NuGet packages to net10-compatible versions

Update packages identified by the assessment (e.g., Newtonsoft.Json to a supported 13.x or newer compatible version, replace deprecated Microsoft.Extensions.CommandLineUtils with maintained alternatives). Prefer PackageReference and ensure package versions support net10.0. Re-run restore and resolve any package-reference conflicts.

Affected items: packages referenced by DatabaseSerialiser.csproj

Done when: All referenced packages resolve and restore for net10.0 and no package incompatibility issues remain.
