# Migration Progress

**Progress**: 2/9 tasks complete <progress value="22" max="100"></progress> 22%
**Status**: In Progress - Task 01-convert-to-sdk

## Tasks

- 🔄 01-convert-to-sdk: Convert DatabaseSerialiser.csproj to SDK-style ([Content](tasks/01-convert-to-sdk/task.md))
  - ✅ 01.01-analyze-project: Analyze project and dependencies before conversion ([Content](tasks/01.01-analyze-project/task.md), [Progress](tasks/01.01-analyze-project/progress-details.md))
  - ✅ 01.02-convert-project-file: Convert project file to SDK-style and migrate package references ([Content](tasks/01.02-convert-project-file/task.md), [Progress](tasks/01.02-convert-project-file/progress-details.md))
  - 🔲 01.03-validate-and-commit: Restore, build and validate project after conversion, then commit
- 🔲 02-update-tfms-and-project-file: Update project target framework(s) to net10.0 ([Content](tasks/02-update-tfms-and-project-file/task.md))
- 🔲 03-update-nuget-packages: Upgrade NuGet packages to net10-compatible versions ([Content](tasks/03-update-nuget-packages/task.md))
- 🔲 04-fix-binding-redirects: Remove or correct binding redirects and assembly binding policies ([Content](tasks/04-fix-binding-redirects/task.md))
- 🔲 05-migrate-sqlclient-apis: Replace System.Data.SqlClient usages with Microsoft.Data.SqlClient or compatible APIs ([Content](tasks/05-migrate-sqlclient-apis/task.md))
- 🔲 06-build-and-integration-validation: Build solution, run tests, and validate behavior ([Content](tasks/06-build-and-integration-validation/task.md))
- 🔲 07-finalize-and-cleanup: Final commits, documentation and report ([Content](tasks/07-finalize-and-cleanup/task.md))

**Legend**: ✅ Complete | 🔄 In Progress | 🔲 Pending | ⚠️ Blocked | ❌ Failed
