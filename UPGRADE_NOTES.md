# Upgrade to .NET 10

This repository was upgraded from .NET Framework 4.7.2 to .NET 10.

Summary of changes:

- Converted DatabaseSerialiser.csproj to SDK-style
- Migrated packages to PackageReference
- Added Microsoft.Data.SqlClient and migrated SqlClient usages
- Updated Newtonsoft.Json to 13.0.4 and fixed binding redirects
- Validated builds for net10.0 and net472 during migration

Notes:
- Microsoft.Extensions.CommandLineUtils is deprecated but still referenced; consider replacing with a maintained alternative.
- Some NuGet packages report advisories (Azure.Identity, Microsoft.Identity.Client). Review and update if required.
