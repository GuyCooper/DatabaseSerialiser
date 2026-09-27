# Progress details for 04-fix-binding-redirects

What I did:

- Updated App.config binding redirect for Newtonsoft.Json to newVersion 13.0.4.0 and oldVersion range 0.0.0.0-13.0.4.0.
- Built the solution for net472 and net10.0 to verify no binding redirect conflicts remain.

Files modified:
- App.config
- .github/upgrades/scenarios/dotnet-version-upgrade/tasks/04-fix-binding-redirects/progress-details.md

Notes:
- MSBuild warning MSB3277 previously indicated a conflict between Newtonsoft.Json versions; updating the binding redirect resolved the conflict at runtime. Build now succeeds with warnings unrelated to redirects.

Next: complete this task and proceed to task 06 (build and integration validation).