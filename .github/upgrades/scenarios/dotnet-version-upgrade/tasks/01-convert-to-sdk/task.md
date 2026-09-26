# 01-convert-to-sdk: Convert DatabaseSerialiser.csproj to SDK-style

Convert the existing classic (non-SDK) project file to the SDK-style project format. Replace packages.config or legacy references with PackageReference entries where applicable, remove legacy assembly binding redirects from the project, and simplify the project file to a minimal SDK-style manifest.

Affected items: DatabaseSerialiser.csproj

Done when: The project file is an SDK-style .csproj, restores packages with dotnet restore, and the project builds targeting net10.0 when temporarily multi-targeting (see task 02) or after TFM update.
