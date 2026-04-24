# 000 — Bootstrap solution

Create the .NET solution and projects per `AGENTS.md` layout. Nothing
else. No domain code yet.

## Items

- [x] Create `ChessEngine.sln` with three projects:
  `src/ChessEngine/ChessEngine.csproj` (classlib, net8.0),
  `src/ChessEngine.Cli/ChessEngine.Cli.csproj` (exe, net8.0,
  references ChessEngine),
  `tests/ChessEngine.Tests/ChessEngine.Tests.csproj` (xunit,
  references ChessEngine). All three have `<Nullable>enable</Nullable>`
  and `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
- [x] Add one smoke test in `tests/ChessEngine.Tests/SmokeTests.cs`:
  `Assert.True(true)`. Confirms `dotnet test` works end-to-end.
- [x] `ChessEngine.Cli/Program.cs` prints `"ChessEngine vNEXT"` and exits 0.
