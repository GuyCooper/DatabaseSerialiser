# .NET Version Upgrade

## Strategy
Convert project to SDK-style, update project TFM to net10.0, update NuGet packages, fix binding redirects, and address source-incompatible APIs (SqlClient). Execute tasks in sequence with validation and commits after each task.

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0 (.NET 10, LTS)

## Source Control
- **Source Branch**: upgrade-to-net10
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Key Decisions Log
- Confirmed upgrade target: net10.0 (user requested upgrade to .NET 10)
