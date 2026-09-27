# 05-migrate-sqlclient-apis: Replace System.Data.SqlClient usages with Microsoft.Data.SqlClient or compatible APIs

Update code that uses System.Data.SqlClient types where source incompatibilities were flagged. Replace with Microsoft.Data.SqlClient usage or adjust calls to match API differences. Run targeted builds and unit tests for affected code paths.

Affected items: source files using SqlClient (see assessment.md for list)

Done when: Code compiles for net10.0 and no source-incompatible API errors remain for SqlClient-related code.
