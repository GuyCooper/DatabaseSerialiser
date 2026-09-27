# 04-fix-binding-redirects: Remove or correct binding redirects and assembly binding policies

Remove conflicting manual binding redirects from configuration and let SDK/MSBuild manage redirects, or update redirect versions to match package versions. Ensure app.config/web.config contains no redirects that force incompatible versions.

Affected items: app.config (or other config files), project properties

Done when: No binding-redirect conflicts reported by the build and runtime assembly loading is consistent.
