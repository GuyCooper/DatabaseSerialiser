# 01.02-convert-project-file: Convert project file to SDK-style and migrate package references

## Objective
Create an SDK-style version of DatabaseSerialiser.csproj preserving references and package versions by converting packages.config to PackageReference where needed and simplifying project imports.

## Actions performed
- Backed up original DatabaseSerialiser.csproj to DatabaseSerialiser.csproj.backup
- Created SDK-style DatabaseSerialiser.csproj with TargetFramework net472 and PackageReference entries for:
  - Microsoft.Extensions.CommandLineUtils (1.1.1)
  - Newtonsoft.Json (13.0.1)
  - Newtonsoft.Json.Bson (1.0.2)
- Disabled SDK auto-generation of assembly attributes to preserve existing Properties\AssemblyInfo.cs

## Done when
- New SDK-style .csproj exists and the solution builds without errors.
