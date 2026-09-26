# 01.01-analyze-project: Analyze project and dependencies before conversion

## Objective
Inventory DatabaseSerialiser.csproj: list PackageReferences, packages.config (if present), project references, build targets, and any custom MSBuild imports.

## Scope Inventory
- Projects affected: DatabaseSerialiser.csproj
- Concerns: project format (classic non-SDK), package management mode (packages.config present), binding redirects configured via App.config, custom MSBuild imports (Microsoft.Common.props/targets)

## Findings (from DatabaseSerialiser.csproj)
- Project is classic (ToolsVersion="14.0"), not SDK-style.
- TargetFrameworkVersion: v4.7.2
- packages.config is present (legacy NuGet)
- References with HintPath to packages folder:
  - Microsoft.Extensions.CommandLineUtils (packages\Microsoft.Extensions.CommandLineUtils.1.1.1)
  - Newtonsoft.Json (packages\Newtonsoft.Json.13.0.1)
  - Newtonsoft.Json.Bson (packages\Newtonsoft.Json.Bson.1.0.2)
- No ProjectReference entries found (single-project solution)
- App.config exists (binding redirects may be present) — review required (assessment flagged binding redirect conflicts for Newtonsoft.Json)
- MSBuild imports: Microsoft.Common.props and Microsoft.CSharp.targets (standard for classic projects)

## Potential Blockers
- Manual binding redirect for Newtonsoft.Json targets 13.0.0.0 conflicting with package-provided 13.0.1 (assessment flagged rule Binding.0006/0007)
- Microsoft.Extensions.CommandLineUtils is deprecated — replacement or removal may be required
- Packages.config requires migration to PackageReference for SDK-style conversion

## Next steps
- Convert packages.config to PackageReference or preserve package references during SDK conversion
- Prepare a backup of the original .csproj before editing

## Done when
- A clear inventory exists listing the legacy project sections to migrate and any potential blockers (this file)
