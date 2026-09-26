# 01.03-validate-and-commit: Restore, build and validate project after conversion, then commit

## Objective
Restore packages, build the project, run smoke tests, and commit changes per commit strategy.

## Done when
- `dotnet restore`/msbuild restore succeeds, project builds without errors and warnings, basic serialization functionality verified, and changes committed to the working branch.
